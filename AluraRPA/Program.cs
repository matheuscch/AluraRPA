// See https://aka.ms/new-console-template for more information
using EasyAutomationFramework;
using System.Data.SqlClient;

//CONNECTION STRING DATABASE
string cs = "datasource=INPUT_YOUR_DATASOURCE_PREVIOUS_CREATED.;database = RPADB;Integrate Security=true;";

Console.WriteLine("Qual curso?");
string? cursoSearchField = Console.ReadLine();

var web = new Web();

var itemPage = 1;
while (itemPage <= 25)
{
    web.StartBrowser();
    web.Navigate("https://www.alura.com.br/busca?query=" + cursoSearchField + "&typeFilters=COURSE");

    var scrapName = web.GetValue(TypeElement.Xpath, "/html/body/div[1]/div[2]/section/ul/li[" + itemPage + "]/a/div/h4");
    var scrapDescricao = web.GetValue(TypeElement.Xpath, "/html/body/div[1]/div[2]/section/ul/li[" + itemPage + "]/a/div/p");
    if (scrapName.Error == null && scrapName.Value != null)
    {
        web.Click(TypeElement.Xpath, "/html/body/div[1]/div[2]/section/ul/li[" + itemPage + "]/a/div/h4");

        var scrapCargaHoraria = web.GetValue(TypeElement.Xpath, "/html/body/section[1]/div/div[2]/div[1]/div/div[1]/div/p[1]");
        var scrapProfessor = web.GetValue(TypeElement.Xpath, "/html/body/section[2]/div[1]/section/div/div/div/h3");

        Console.WriteLine("Titulo:" + scrapName.Value);
        Console.WriteLine("Descricao:" + scrapDescricao.Value);
        Console.WriteLine("Carga Horaria:" + scrapCargaHoraria.Value);
        Console.WriteLine("Professor:" + scrapProfessor.Value + "\r\n");

        web.CloseBrowser();

        Console.WriteLine("Iniciando query INSERT no banco de dados." + "\r\n");

        try
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            string query = string.Format("insert into Cursos values({0},{1},{2},{3})", scrapName.Value, scrapDescricao.Value, scrapCargaHoraria.Value, scrapProfessor.Value);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Close();
        }
        catch
        {

            Console.WriteLine("Houve um problema ao armazenar no banco de dados." + "\r\n");
        }
        itemPage++;
    }
    else
    {
        itemPage = 26;
        web.CloseBrowser();
    }
}






