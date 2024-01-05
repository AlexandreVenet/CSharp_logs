using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharp_logs
{
	internal class Logs
	{
		#region Champs

		private string _cheminDossierLogs;
		private string _cheminFichierActuel;
		private char _separateurNomFichier = '_';

		private string _separateur = " ";
		private string _formatDate = "yyyy-MM-dd HH:mm:ss.fff";
		private string _prefixe;
		private int _titreMaxCaracteres = 15;
		private int _titreMaxCaracteresAutorises = 30;

		#endregion



		#region Constructeur

		public Logs(string cheminDossierLogs)
		{
			VerifierCheminDossierLogs(cheminDossierLogs);

			CreerRepertoireLog();
			DefinirCheminFichier();
		}

		public Logs(LogsParametres parametres)
		{
			VerifierCheminDossierLogs(parametres.CheminDossierLogs);
			VerifierPrefixe(parametres.Prefixe);
			VerifierFormatDate(parametres.FormatDate);
			VerifierTitreMaxCaracteres(parametres.TitreMaxCaracteres);

			CreerRepertoireLog();
			DefinirCheminFichier();
		}

		#endregion



		#region Méthodes

		private void VerifierCheminDossierLogs(string cheminDossierLogs)
		{
			string fullPath = Path.GetFullPath(cheminDossierLogs);

			if (!Path.IsPathRooted(fullPath) || Path.HasExtension(fullPath))
			{
				throw new ArgumentException("Le chemin de dossier est invalide.");
			}

			_cheminDossierLogs = cheminDossierLogs;
		}

		private void VerifierPrefixe(string prefixe)
		{
			if (string.IsNullOrEmpty(prefixe))
			{
				return; // _prefixe reste null
			}

			string motif = @"^[a-zA-Z0-9_-]{0,8}$";
			bool correspond = Regex.IsMatch(prefixe, motif);
			if (!correspond)
			{
				throw new ArgumentException($"Le préfixe est invalide. Utiliser : {motif}");
			}

			_prefixe = prefixe;
		}

		private void VerifierFormatDate(string formatDate)
		{
			if (string.IsNullOrEmpty(formatDate))
			{
				return; // _formatDate reste à sa valeur par défaut
			}

			_formatDate = formatDate;
		}

		private void VerifierTitreMaxCaracteres(int titreMaxCaracteres)
		{
			if (titreMaxCaracteres > _titreMaxCaracteresAutorises)
			{
				throw new ArgumentException($"Le nombre maximum de caractères autorisés pour le titre est {_titreMaxCaracteresAutorises}.");
			}

			if (titreMaxCaracteres < 0)
			{
				throw new ArgumentException("Le nombre maximum de caractères autorisés ne peut pas être négatif.");
			}

			// On peut paramétrer à 0 caractères. ^^

			_titreMaxCaracteres = titreMaxCaracteres;
		}

		private void CreerRepertoireLog()
		{
			if (!Directory.Exists(_cheminDossierLogs))
			{
				Directory.CreateDirectory(_cheminDossierLogs);
			}
		}

		private void DefinirCheminFichier()
		{
			DateTime now = ObtenirMaintenant();

			StringBuilder sb = new();

			if (!string.IsNullOrEmpty(_prefixe) && !string.IsNullOrWhiteSpace(_prefixe))
			{
				sb.Append(_prefixe);
				sb.Append(_separateurNomFichier);
			}

			sb.Append(now.Year.ToString("0000"));
			sb.Append(_separateurNomFichier);
			sb.Append(now.Month.ToString("00"));
			sb.Append(_separateurNomFichier);
			sb.Append(now.Day.ToString("00"));
			sb.Append(_separateurNomFichier);
			sb.Append(now.Hour.ToString("00"));
			sb.Append(now.Minute.ToString("00"));
			sb.Append(now.Second.ToString("00"));
			sb.Append(now.Millisecond.ToString("000"));
			sb.Append(".txt");

			_cheminFichierActuel = Path.Combine(_cheminDossierLogs, sb.ToString());
		}

		private DateTime ObtenirMaintenant()
		{
			return DateTime.Now;
		}

		public void Log(string titre, string message)
		{
			EcrireLog(ObtenirMaintenant().ToString(_formatDate), DefinirTitre(titre), message);
		}

		public void Log(string message)
		{
			EcrireLog(ObtenirMaintenant().ToString(_formatDate), null, message);
		}

		private void EcrireLog(string date, string titre, string message)
		{
			StringBuilder sb = new();

			sb.Append(date);

			if (!string.IsNullOrEmpty(titre) && !string.IsNullOrWhiteSpace(titre))
			{
				sb.Append(_separateur);
				sb.Append(titre);
			}

			if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
			{
				sb.Append(_separateur);
				sb.Append(message);
			}

			using (StreamWriter sw = new(_cheminFichierActuel, append: true))
			{
				sw.WriteLine(sb);
				sw.Flush();
			}
		}

		private string DefinirTitre(string titre)
		{
			string titreOk = string.Empty;

			if (string.IsNullOrEmpty(titre))
			{
				titreOk = "-";
			}
			else if (titre.Length > _titreMaxCaracteres)
			{
				titreOk = titre.Substring(0, _titreMaxCaracteres);
			}
			else
			{
				titreOk = titre;
			}

			titreOk = titreOk.PadRight(_titreMaxCaracteres);

			return titreOk;
		}

		#endregion
	}
}
