
using PaddleOCRSharp.VL;

namespace PaddleOCRSharp_VL_1._5_Demo
{
    internal class Program
    {
        private static PaddleOCRVLEngine engine;
        static void Main(string[] args)
        {
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
            Console.ReadKey();
        }
    }
}
