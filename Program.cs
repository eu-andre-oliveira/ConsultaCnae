using System.Diagnostics.Metrics;
using System.Net.Http.Json;

namespace ConsultaCnae
{
    public record AtividadePrincipal(string code, string text);

    public record Empresa(AtividadePrincipal[] atividade_principal);

    public class Program
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task Main(string[] args)
        {
            int count = 0;
            List<string> cnpjs = new List<string>() { "33014556009819", "00000000000191", "20250105000106", "50441644000193" };
            foreach (var cnpj in cnpjs)
            {


                //string cnpj = "00000000000191"; // Insira o CNPJ desejado
                if (count < 3)
                    count++;
                else
                {
                    count = 0;
                    Thread.Sleep(60000);
                }


                var empresa = await ConsultarCnpj(cnpj);

                if (empresa != null && empresa.atividade_principal.Length > 0)
                {
                    var atividadePrincipal = empresa.atividade_principal[0];
                    Console.WriteLine($"CNPJ: {cnpj}");
                    Console.WriteLine($"CNAE: {atividadePrincipal.code}");
                    Console.WriteLine($"Descrição: {atividadePrincipal.text}");
                }
                else
                {
                    Console.WriteLine("Atividade principal não encontrada ou erro na consulta.");
                }
            }
        }

        public static async Task<Empresa> ConsultarCnpj(string cnpj)
        {
            try
            {
                string url = $"https://www.receitaws.com.br/v1/cnpj/{cnpj}";
                var empresa = await client.GetFromJsonAsync<Empresa>(url);
                return empresa;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na consulta: {ex.Message}");
                return null;
            }
        }
    }
}