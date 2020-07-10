using System;
using System.Net;

namespace MiniHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //httpListener提供一个简单，可通过编程方式控制的Http协议侦听器。此类不能被继承。
            if (!HttpListener.IsSupported)
            {
                //该类只能在Windows xp sp2或者Windows server 200 以上的操作系统中才能使用，因为这个类必须使用Http.sys系统组件才能完成工作                
                //。所以在使用前应该先判断一下是否支持该类
                Console.WriteLine("Windows xp sp2 or server 2003 is required to use the HttpListener class");
            }
            //设置前缀，必须以‘/’结尾
            string[] prefixes = new string[] { "http://localhost:8888/" };
            //初始化监听器
            HttpListener listener = new HttpListener();
            //将前缀添加到监听器
            foreach (var item in prefixes)
            {
                listener.Prefixes.Add(item);
            }
            //判断是否已经启动了监听器,如果没有则开启
            if (!listener.IsListening)
            {
                listener.Start();
            }
            //提示
            Console.WriteLine("服务器已经启动，开始监听....");

            while (true)
            {
                //等待传入的请求，该方法将阻塞进程，直到收到请求
                HttpListenerContext context = listener.GetContext();
                //取得请求的对象
                HttpListenerRequest request = context.Request;
                //打印状态行 请求方法，url，协议版本
                Console.WriteLine("{0} {1} HTTP/{2}\r\n", request.HttpMethod, request.RawUrl, request.ProtocolVersion);
                //打印接收类型
                Console.WriteLine("Accept: {0}", string.Join(",", request.AcceptTypes));
                //打印接收语言
                Console.WriteLine("Accept-Language: {0}", string.Join(",", request.UserLanguages));
                //打印编码格式
                Console.WriteLine("Accept-Encoding: {0}", request.Headers["Accept-Encoding"]);
                //客户端引擎
                Console.WriteLine("User-Agent: {0}", request.UserAgent);
                //是否长连接
                Console.WriteLine("Connection: {0}", request.KeepAlive ? "Keep-Alive" : "close");
                //客户端主机
                Console.WriteLine("Host: {0}", request.UserHostName);
                Console.WriteLine("Pragma: {0}", request.Headers["Pragma"]);
                //取得响应对象
                HttpListenerResponse response = context.Response;
                //构造响应内容                
                //准备发送到客户端的网页
                string responseBody = "<html><head><title>这是一个web服务器的测试</title></head><body><h1>Hello World.</h1></body></html>";
                //设置响应头部内容，长度及编码
                response.ContentLength64 = System.Text.Encoding.UTF8.GetByteCount(responseBody);
                response.ContentType = "text/html; Charset=UTF-8";
                //输出响应内容
                System.IO.Stream output = response.OutputStream;
                System.IO.StreamWriter sw = new System.IO.StreamWriter(output);
                sw.Write(responseBody);
                sw.Dispose(); break;
            }
            //关闭服务器            

            listener.Close();
            Console.Read();

        }
    }
}