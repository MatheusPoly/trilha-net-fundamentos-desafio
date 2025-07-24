using System.Text.RegularExpressions;

namespace DesafioFundamentos.Models
{
    public class Estacionamento
    {
        private decimal precoInicial = 0;
        private decimal precoPorHora = 0;
        private Dictionary<string, DateTime> veiculos = new Dictionary<string, DateTime>();

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
        }

        public void AdicionarVeiculo()
        {   
            Regex regexPlaca = new Regex(@"^([A-Z]{3}-\d{4}|[A-Z]{3}\d[A-Z]\d{2})$", RegexOptions.IgnoreCase);
            string placa;
            DateTime dataAtual = DateTime.Now;

            do //Pede para o usuário digitar uma placa nos formatos XXX-0000 (formato antigo) ou XXX0X00 (mercosul)
            {
                Console.WriteLine("Digite a placa do veículo (formatos válidos: 'ABC-1234' ou 'ABC1D23'):");
                placa = Console.ReadLine();

                if (!regexPlaca.IsMatch(placa))
                {
                    Console.WriteLine("Formato inválido. A placa deve estar em um dos formatos: 'ABC-1234' ou 'ABC1D23'. Tente novamente.");
                    break;
                }

            }

            while (!regexPlaca.IsMatch(placa));
            placa = placa.ToUpper();

            if (veiculos.ContainsKey(placa))
            {
                Console.WriteLine("Este veículo já está estacionado");
            }

            veiculos.Add(placa, DateTime.Now); // Para manter padrão
            Console.WriteLine($"Placa '{placa.ToUpper()}' registrada com sucesso.");
        }


        public void RemoverVeiculo()
        {
            if (veiculos.Any())  // Mostra a lista de veículos para seleção antes da remoção       
            {
                int contador = 0;
            
                foreach (var v in veiculos)
                {
                    Console.WriteLine($"{contador + 1} - Placa: {v.Key} | Entrada: {v.Value:HH:mm:ss}");
                    contador++;
                }
            }

            else
            {
                Console.WriteLine("Não há veículos estacionados.");
            }

            Console.WriteLine("Digite a placa do veículo que deseja dar baixa:");
            string placa = Console.ReadLine().ToUpper();

            // Contabiliza a quantidade de horas estacionado, calculando o valor total a ser pago
            if (veiculos.ContainsKey(placa))
            {
                DateTime horaEntrada = veiculos[placa];
                DateTime horaSaida = DateTime.Now;
                TimeSpan tempoEstacionado = horaSaida - horaEntrada;

                if (tempoEstacionado.TotalMinutes <= 1)
                {
                    Console.WriteLine("O veículo ficou até 10 minutos. Tolerância gratuita aplicada.");
                    veiculos.Remove(placa);
                    return;
                }

                // Calcula horas completas estacionadas
                int horasTotais = (int)tempoEstacionado.TotalHours;

                // Cobra sempre o valor inicial pela primeira hora (ou fração), após a tolerância de 10 minutos
                decimal valorTotal = precoInicial;

                // Se passou de 1 hora, cobra horas adicionais
                if (horasTotais >= 1)
                {
                    valorTotal += precoPorHora * (horasTotais - 1);
                }

                veiculos.Remove(placa);

                Console.WriteLine($"\nVeículo {placa} removido:");
                Console.WriteLine($"Hora de entrada: {horaEntrada:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine($"Hora de saída  : {horaSaida:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine($"Tempo total    : {tempoEstacionado.TotalMinutes:F0} minuto(s)");
                Console.WriteLine($"Valor total    : R$ {valorTotal:F2}");
            }

            else
            {
                Console.WriteLine("Desculpe, esse veículo não está estacionado aqui.");
            }
        }

        public void ListarVeiculos()
        {
            // Retorna a lista de veículos cadastrados no estacionamento
            if (veiculos.Any())            
            {

                int contador = 0;
            
                foreach (var v in veiculos)
                {
                    Console.WriteLine($"{contador + 1} - Placa: {v.Key} | Entrada: {v.Value:hh:mm:ss}");
                    contador++;
                }
            }

            else
            {
                Console.WriteLine("Não há veículos estacionados.");
            }
        }
    }
}
