# PaddleOCRSharp-VL-1.5

PaddleOCRSharp-VL-1.5 is the PaddleOCR-VL-1.5 optimized encapsulation library, supports:OCR;Table Recognition;Formula Recognition;Chart Recognition;Spotting;Seal Recognition

support for LLM for example: DeepSeek、Qwen ...

# How to use

1. Install the Nuget package PaddleOCRSharp-VL-1.5 or download  https://www.nuget.org/packages/PaddleOCRSharp-VL-1.5
 
2. Download the model file,  from the following url:

https://www.modelscope.cn/models/megemini/PaddleOCR-VL-1.5-GGUF

Or LLM model

https://www.modelscope.cn/models/duanzy1/Qwen3.5-9B_guff/files


3. Code

```
using PaddleOCRSharp.VL;

string root = AppContext.BaseDirectory;
root += @"\models\";
//下载模型，假设放在models目录下
string model_file = Path.Combine(root, "PaddleOCR-VL-1.5-GGUF.gguf");
//对话大模型，该字段为空
string mmproj_file = Path.Combine(root, "PaddleOCR-VL-1.5-GGUF-mmproj.gguf");

//创建引擎对象
engine = new PaddleOCRVLEngine();
try
    {
      //加载模型，参数为全路径,mmproj_file对话大模型，该字段为空即可
      engine.LoadModel(model_file, mmproj_file);
      DateTime dt1 = DateTime.Now;
     //加载图片,对话大模型，不用加载图片
     engine.LoadImage("test.jpg")
      //根据提示词，处理图片
      string ocrResult = engine.Generate(Prompt.OCR);
      DateTime dt2 = DateTime.Now;
      var times = (dt2 - dt1).TotalMilliseconds;
      Console.WriteLine($"result:\n{ocrResult}\ntimes:{times}ms");
    }
catch (Exception ex)
    {
      Console.WriteLine($"error:\n{ex.Message}");        
    }
finally
    {
      //按需释放资源
      //engine?.Release();
    }
     Console.ReadKey();

```

4. note

Each Generate does not retain context and historical records


