namespace CSharp_logs
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Lancer();
		}

		private static void Lancer()
		{
			string cheminDuProgramme = AppDomain.CurrentDomain.BaseDirectory;
			//string cheminDuProgramme = Directory.GetCurrentDirectory(); // idem
			string nomDossierLogs = "Logs";
			string cheminLogs = Path.Combine(cheminDuProgramme, nomDossierLogs);

			// V.1 : utiliser les valeurs par défaut
			//Logs logs = new(cheminLogs); // ok à la racine de l'app

			// V.2 : configurer des valeurs (tout ou parties)
			LogsParametres logsParametres = new()
			{
				CheminDossierLogs = nomDossierLogs, // ok à la racine de l'app
				Prefixe = "Test",
				FormatDate = "dd/MM/yyyy HH:mm:ss.fff",
				TitreMaxCaracteres = 20 // 15 par défaut, 30 max
			};
			Logs logs = new(logsParametres);

			logs.Log("Test", "Premier message.");
			logs.Log("Second message", "Je suis le second message.");
			logs.Log("Autre", "");
			logs.Log("", "");
			logs.Log("", null);
			logs.Log(null, "");

			logs.Log("Pas de titre, seulement un message.");
			logs.Log(null);
			logs.Log("Fin du fichier.");
		}
	}
}
