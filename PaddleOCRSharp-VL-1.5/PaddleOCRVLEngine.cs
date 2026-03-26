// Copyright (c) 2026 raoyutian. All Rights Reserved.
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

using System;
using System.Runtime.InteropServices;

namespace PaddleOCRSharp.VL
{
    /// <summary>
    /// PaddleOCRVLEngine文字识别引擎对象
    /// </summary>
    public class PaddleOCRVLEngine : EngineBase
    {
        #region PaddleOCRVL API
         /// <summary>
         /// 非托管推理引擎实例内存地址
         /// </summary>
         protected IntPtr enginePtr { get; set; }

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr llama_init(string model_file, string mmproj_file, llama_Params llama_params);
        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr llama_initjson(string model_file, string mmproj_file, string llama_params_json);
       
        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr llama_generate(IntPtr engine_ptr, string prompt, string imagefile);

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern void llama_release(IntPtr engine_ptr);

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern void llama_free_string(string str);

        #endregion

        #region 文本识别
        /// <summary>
        /// 使用默认参数初始化引擎对象
        /// </summary>
        public PaddleOCRVLEngine()
        {
            enginePtr = IntPtr.Zero;
        }
        /// <summary>
        /// 加载模型
        /// </summary>
        /// <param name="model_file">paddleocr-vl*.guff模型文件</param>
        /// <param name="mmproj_file">paddleocr-vl-mmproj*.guff多模态模型文件</param>
        public void LoadModel(string model_file, string mmproj_file)
        {
            LoadModel(model_file, mmproj_file, new llama_Params());
        }

        /// <summary>
        /// 加载模型
        /// </summary>
        /// <param name="model_file">paddleocr-vl*.guff模型文件</param>
        /// <param name="mmproj_file">paddleocr-vl-mmproj*.guff多模态模型文件</param>
        /// <param name="llama_params">llama_params参数</param>
        public void  LoadModel(string model_file, string mmproj_file, llama_Params llama_params)
        {
            if (llama_params==null) llama_params=new llama_Params();
            enginePtr = llama_init(model_file, mmproj_file, llama_params);
            if (enginePtr == IntPtr.Zero) throw new Exception("Initialize err：" + GetLastError());
        }
        /// <summary>
        /// 加载模型()
        /// </summary>
        /// <param name="model_file">paddleocr-vl*.guff模型文件</param>
        /// <param name="mmproj_file">paddleocr-vl-mmproj*.guff多模态模型文件</param>
        /// <param name="llama_params_json">llama_params参数的json格式字符串</param>
        public void LoadModel(string model_file, string mmproj_file, string llama_params_json)
        {
            enginePtr = llama_initjson(model_file, mmproj_file, llama_params_json);
            if (enginePtr == IntPtr.Zero) throw new Exception("Initialize err：" + GetLastError());
        }

        /// <summary>
        /// 根据prompt提示词处理
        /// </summary>
        /// <param name="prompt">提示词</param>
        /// <param name="imagefile">图像文件</param>
        /// <returns></returns>
        public string Generate(string prompt, string imagefile)
        {
            ZeroThrow(enginePtr);
            var ptrResult = llama_generate(enginePtr, prompt, imagefile);
            return ConvertResult(ptrResult);
        }
        /// <summary>
        /// 结果解析
        /// </summary>
        /// <param name="ptrResult"></param>
        /// <returns></returns>
        private string ConvertResult(IntPtr ptrResult)
        {
            string result = "";
            if (ptrResult == IntPtr.Zero)
            {
                var err = GetLastError();
                if (!string.IsNullOrEmpty(err))
                {
                    throw new Exception("PaddleOCR-VL Err：" + err);
                }
                return result;
            }
            try
            {

#if NET6_0_OR_GREATER
  if (!OperatingSystem.IsWindows())
  {
  result = Marshal.PtrToStringAnsi(ptrResult);
  result=result.Replace("\\r","");
  }
  else
  {
  result = Marshal.PtrToStringUTF8(ptrResult);
  }
#else
                result = Marshal.PtrToStringAnsi(ptrResult);
#endif
            }
            catch (Exception ex)
            {
                throw new Exception("结果解析失败。", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrResult); 
            }
            return result;
        }

        #endregion

        #region Release
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Release()
        {
            llama_release(enginePtr);
            enginePtr = IntPtr.Zero;
        }
        #endregion
    }
}