using System.Text;

HtmlWeb web = new HtmlWeb();
web.OverrideEncoding = Encoding.UTF8;
HtmlDocument doc = new HtmlDocument();

string LinkInLastNews = ""; // ссылка на последнюю новость
string title = ""; // название статьи
PageNode pageNode = new PageNode();

bool isHaveImg = false; // имеется ли картинка на сайте
string UrlToImg = "";

StreamWriter SaveLinkInFile; // сохранение в файл ссылки
StreamWriter logger; // логгер
StreamReader readfile; // чтение из файла 

Bot bot = new Bot();


while (true)
{
    
    doc = web.Load("https://www.061.ua/"); // обновляем сайт 

    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[contains(@class, 'news-feed')]//a[@href]")) // получение ссылки на последнюю новость
    {
        LinkInLastNews = "https://www.061.ua/" + node.GetAttributeValue("href", null);
        break;
    }

    readfile = new StreamReader("links.txt");
    string readlink = readfile.ReadLine(); // считывание последней ссылки

    if (!LinkInLastNews.Equals(readlink)) // если ссылки разные - новая новость
    {
        logger = new StreamWriter("logger.txt", true, Encoding.UTF8);
        string TelegraphLink = ParseNews(); // получение ссылки на телеграф статью
        logger.WriteLine("NEW NEWS|" + DateTime.Now.ToString() + "|LINK: " + TelegraphLink); // запись новой новости в логгер

        if (!isHaveImg) // рассылка
        {
            bot.sendMsg("https://static8.depositphotos.com/1338574/829/i/600/depositphotos_8292496-stock-photo-news.jpg", title, TelegraphLink);
            Console.WriteLine("Send without img!");
        }
        else
        {
            bot.sendMsg(UrlToImg, title, TelegraphLink);
            Console.WriteLine("Send with img!");
        }

        logger.Close();
        readfile.Close();

        SaveLinkInFile = new StreamWriter("links.txt", false, Encoding.UTF8); // записываем новую последнюю новость 
        SaveLinkInFile.WriteLine(LinkInLastNews);
        SaveLinkInFile.Close();

        SaveLinkInFile.Close();
    }



    for (int i = 0; i <= 6; i++) // перерыв - минута
    {
        Thread.Sleep(10000);
    }
}

string ParseNews()
{
    List<string> words = new(); // контент статьи
    doc = web.Load(LinkInLastNews);

    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='article-details__title-container']//h1")) // заголовок
    {
        title += node.InnerText;
    }

    foreach (HtmlNode text in doc.DocumentNode.SelectNodes("//div[@class='article-details__text']//p")) // контент
    {
        words.Add(text.InnerText);
    }

    char[] chars = new char[30];
    int counterfortext = 0;
    int count = 0;
    pageNode.children = "";

    foreach (string str in words) // убираем лишние символы
    {
        chars = str.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '&')
            {
                if (i != chars.Length - 6)
                {
                    i += 6;

                }
                break;
            }
            if (chars[i] == '"')
            {
                pageNode.children += @"\";

            }

            pageNode.children += chars[i];
        }
        pageNode.children += '\n';
        pageNode.children += '\n';
    }

    CreatePage request = new();

    
    isHaveImg = false;
    UrlToImg = null;
    if (UrlToImage() != null)
    {
        UrlToImg = UrlToImage();
        isHaveImg = true;
    }

    string result = request.CreatePageFunc(pageNode, title);

    return result;
}

string UrlToImage() // достаём ссылку на картинку 
{
    HtmlDocument img = web.Load(LinkInLastNews);

    var im = img.DocumentNode.SelectSingleNode("/html/body/script[8]");
    string str = im.OuterHtml;
    string s = "internalPoster&q;:{&q;url&q;:&q;";
    string tmp = str.Substring(str.IndexOf(s) + s.Length, str.Length - (str.IndexOf(s) + s.Length));
    string result = tmp.Substring(0, tmp.IndexOf("&"));


    if (result.Contains("https://s.061.ua/img"))
    {
        return result;
    }
    else
    {
        return null;
    }
}
