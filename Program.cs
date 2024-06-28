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
            string cnpj = "00000000000191"; // Insira o CNPJ desejado
            var empresa = await ConsultarCnpj(cnpj);

            if (empresa != null && empresa.atividade_principal.Length > 0)
            {
                var atividadePrincipal = empresa.atividade_principal[0];
                Console.WriteLine($"CNPJ: 00000000000191");
                Console.WriteLine($"CNAE: {atividadePrincipal.code}");
                Console.WriteLine($"Descrição: {atividadePrincipal.text}");
            }
            else
            {
                Console.WriteLine("Atividade principal não encontrada ou erro na consulta.");
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