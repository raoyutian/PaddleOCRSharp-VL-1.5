# PaddleOCRSharp-VL-1.5

PaddleOCRSharp-VL-1.5 is based on the PaddleOCR-VL-1.5 model and utilizes the llama framework's encapsulation library, supports:OCR;Table Recognition;Formula Recognition;Chart Recognition;Spotting;Seal Recognition

# How to use

1. Install the Nuget package PaddleOCRSharp-VL-1.5 or download  https://www.nuget.org/packages/PaddleOCRSharp-VL-1.5
 
2. Download the model file,  from the following url:

https://huggingface.co/PaddlePaddle/PaddleOCR-VL-1.5-GGUF

https://www.modelscope.cn/models/megemini/PaddleOCR-VL-1.5-GGUF

3.

```
using PaddleOCRSharp.VL;

string root = AppContext.BaseDirectory;
root += @"\models\";
//下载模型，假设放在models目录下
string model_file = Path.Combine(root, "PaddleOCR-VL-1.5-GGUF.gguf");
string mmproj_file = Path.Combine(root, "PaddleOCR-VL-1.5-GGUF-mmproj.gguf");

//创建引擎对象
engine = new PaddleOCRVLEngine();
try
    {
      //加载模型，参数为全路径
      engine.LoadModel(model_file, mmproj_file);
      DateTime dt1 = DateTime.Now;
      //根据提示词，处理图片
      string ocrResult = engine.Generate(Prompt.OCR, "test.jpg");
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

5. todo 

Add API support for LLM for example: DeepSeek、Qwen ...

6. If you find it helpful, please feel free to give us a Star and cite our work.
