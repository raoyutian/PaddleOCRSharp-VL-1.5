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

using System.Runtime.InteropServices;

namespace PaddleOCRSharp.VL
{
    /// <summary>
    /// llama_Params参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class llama_Params
    {

    }

    /// <summary>
    /// OCR内置提示词，根据PaddleOCR-VL-1.5模型
    /// </summary>
    public class Prompt
    {
        /// <summary>
        /// 文字识别
        /// </summary>
        public static string OCR => "OCR:";
        /// <summary>
        /// 表格识别
        /// </summary> 
        public static string Table => "Table Recognition:";
        /// <summary>
        /// 公式识别
        /// </summary> 
        public static string Formula => "Formula Recognition:";
        /// <summary>
        /// 图表转表格
        /// </summary> 
        public static string Chart => "Chart Recognition:";
        /// <summary>
        /// Spotting
        /// </summary> 
        public static string Spotting => "Spotting:";
        /// <summary>
        /// 图章识别
        /// </summary> 
        public static string Seal => "Seal Recognition:";
    }


}


