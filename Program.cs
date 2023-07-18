using System.Net;
using System.Net.Http.Headers;
using System.Timers;

namespace CheckTheWebside
{
    internal class Program
    {
        static System.Timers.Timer timer = new System.Timers.Timer();
        static void Main()
        {

            
            timer.Enabled = true;
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(Sendmessage);

            Console.ReadKey();
        }
        public static bool CheckUrlVisit(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK) return true;
            }
            catch (WebException webex)
            {
                return false;
            }
            return false;
        }
        public static void Sendmessage(object source, EventArgs e)
        {
            // 設定 Line Notify 的權杖（Token）
            string lineNotifyToken;
            using (var reader = new StreamReader("Token.txt"))
            {
                lineNotifyToken = reader.ReadLine();
            }

            HttpClient httpClient = new HttpClient();
            //<form encType=””>中默認的encType，form表單數據被編碼為key/value格式發送到服務器（表單默認的提交數據的格式）
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            // AuthenticationHeaderValue (string scheme 授權的配置, string? parameter  認證);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lineNotifyToken);  
            var content = new Dictionary<string, string>();
            string link = "http://www.google.com";    //測試網址
            content.Add("message", "連結 " + link + " 網站 : " + CheckUrlVisit("http://www.google.com").ToString());
            //使用 application/x-www-form-urlencoded MIME 類型編碼之名稱/值 Tuple 的容器。
            //FormUrlEncodedContent(IEnumerable<KeyValuePair<String,String>>)
            httpClient.PostAsync("https://notify-api.line.me/api/notify", new FormUrlEncodedContent(content));
            

        }


    }
}

