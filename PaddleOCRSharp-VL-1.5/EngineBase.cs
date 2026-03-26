// Copyright (c) 2026 raoyutian  All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace PaddleOCRSharp.VL
{
    /// <summary>
    /// 引擎对象基类
    /// </summary>
    public abstract class EngineBase
    {
        #region DllImport
        /// <summary>
        /// ytllamacore.dll自定义加载路径，默认为空，如果指定则需在引擎实例化前赋值。
        /// </summary>
        public static string dllPath { get; set; }
        internal const string dllName = "ytllamacore";

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr llama_get_error();

       
        #endregion

        #region public
        /// <summary>
        /// 获取程序的当前路径;
        /// </summary>
        /// <returns></returns>
        public static string GetRootDirectory()
        {
            List<string> paths = new List<string>();
            string path1 = AppDomain.CurrentDomain.BaseDirectory;
#if NET461_OR_GREATER || NETCOREAPP || NET6_0_OR_GREATER
            path1 = AppContext.BaseDirectory;
#endif
            string path2 = Path.GetDirectoryName(typeof(EngineBase).Assembly.Location);
            if(!string.IsNullOrEmpty(path1))
            {
                paths.Add(path1);
                paths.AddRange(Directory.GetDirectories(path1, "*").ToList());
            }
            if (!string.IsNullOrEmpty(path2))
            {
                paths.Add(path2);
                paths.AddRange(Directory.GetDirectories(path2, "*").ToList());
            }
            foreach (string path in paths)
            {
                if(string.IsNullOrEmpty(path)) continue;
                
                if (File.Exists(Path.Combine(path, dllName + ".dll")))
                {
                    return path;
                }
            }
            return path1;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public EngineBase()
        {
            try
            {
#if NET6_0_OR_GREATER
             if(OperatingSystem.IsWindows())
             {
                SetEnvironment();
             }
#else
                SetEnvironment();
#endif
            }
            catch (Exception e)
            {
                throw new Exception($" Load the library [{dllName}] fail" + e.Message);
            }
        }
        #endregion

        #region private

        /// <summary>
        /// 设置自动加载dll路径;
        /// </summary>
        /// <returns></returns>
        private static void SetEnvironment()
        {
            if (string.IsNullOrEmpty(dllPath))
            {
                dllPath = GetRootDirectory();
            }
            if (!string.IsNullOrEmpty(dllPath))
            {
                string Envpath = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.Process);
                if (!string.IsNullOrEmpty(Envpath) && !Envpath.EndsWith(dllPath))
                {
                    Environment.SetEnvironmentVariable("path", Envpath + ";" + dllPath, EnvironmentVariableTarget.Process);
                }
            }
        }

        /// <summary>
        /// 获取底层错误信息
        /// </summary>
        /// <returns></returns>
        internal virtual string GetLastError()
        {
            string err = "";
            try
            {
                var errptr = llama_get_error();
                if (errptr != IntPtr.Zero)
                {
                    err = Marshal.PtrToStringAnsi(errptr);
                    Marshal.FreeHGlobal(errptr);
                }
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return err;
        }
        /// <summary>
        /// 检测指针是否为0
        /// </summary>
        /// <returns></returns>
        internal void ZeroThrow(IntPtr enginePtr)
        {
            if (enginePtr == IntPtr.Zero) throw new Exception("Please create an engine object first and LoadModel");
        }
        #endregion
    }
}