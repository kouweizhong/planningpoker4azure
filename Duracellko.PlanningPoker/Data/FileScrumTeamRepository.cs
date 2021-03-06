﻿// <copyright>
// Copyright (c) 2012 Rasto Novotny
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Duracellko.PlanningPoker.Configuration;
using Duracellko.PlanningPoker.Domain;

namespace Duracellko.PlanningPoker.Data
{
    /// <summary>
    /// Repository of scrum teams using file system storage.
    /// </summary>
    public class FileScrumTeamRepository : IScrumTeamRepository
    {
        #region Fields

        private const char SpecialCharacter = '%';
        private const string FileExtension = ".team";

        private readonly IFileScrumTeamRepositorySettings settings;
        private readonly IPlanningPokerConfiguration configuration;
        private readonly DateTimeProvider dateTimeProvider;
        private readonly Lazy<string> folder;
        private readonly Lazy<char[]> invalidCharacters;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileScrumTeamRepository" /> class.
        /// </summary>
        /// <param name="settings">The repository settings.</param>
        /// <param name="configuration">The configuration of the planning poker.</param>
        /// <param name="dateTimeProvider">The date-time provider.</param>
        public FileScrumTeamRepository(IFileScrumTeamRepositorySettings settings, IPlanningPokerConfiguration configuration, DateTimeProvider dateTimeProvider)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;
            this.configuration = configuration ?? new PlanningPokerConfigurationElement();
            this.dateTimeProvider = dateTimeProvider ?? new DateTimeProvider();
            this.folder = new Lazy<string>(this.GetFolder);
            this.invalidCharacters = new Lazy<char[]>(GetInvalidCharacters);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the folder with stored scrum teams.
        /// </summary>
        /// <value>
        /// The storage folder.
        /// </value>
        public string Folder
        {
            get
            {
                return this.folder.Value;
            }
        }

        #endregion

        #region IScrumTeamRepository

        /// <summary>
        /// Gets a collection of Scrum team names.
        /// </summary>
        public IEnumerable<string> ScrumTeamNames
        {
            get
            {
                var expirationTime = this.dateTimeProvider.UtcNow - this.configuration.RepositoryTeamExpiration;
                var directory = new DirectoryInfo(this.Folder);
                if (directory.Exists)
                {
                    var files = directory.GetFiles("*" + FileExtension);
                    foreach (var file in files)
                    {
                        if (file.LastWriteTimeUtc >= expirationTime)
                        {
                            var teamName = this.GetScrumTeamName(file.Name);
                            if (teamName != null)
                            {
                                yield return teamName;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the Scrum team from repository.
        /// </summary>
        /// <param name="teamName">Name of the team.</param>
        /// <returns>The Scrum team with specified name.</returns>
        public ScrumTeam LoadScrumTeam(string teamName)
        {
            if (string.IsNullOrEmpty(teamName))
            {
                throw new ArgumentNullException("teamName");
            }

            string file = this.GetFileName(teamName);
            file = Path.Combine(this.Folder, file);

            ScrumTeam result = null;
            if (File.Exists(file))
            {
                try
                {
                    using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        result = this.DeserializeScrumTeam(stream);
                    }
                }
                catch (IOException)
                {
                    // file is not accessible
                    result = null;
                }
                catch (SerializationException)
                {
                    // file is currupted
                    result = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Saves the Scrum team to repository.
        /// </summary>
        /// <param name="team">The Scrum team.</param>
        public void SaveScrumTeam(ScrumTeam team)
        {
            if (team == null)
            {
                throw new ArgumentNullException("team");
            }

            this.InitializeFolder();

            string file = this.GetFileName(team.Name);
            file = Path.Combine(this.Folder, file);

            using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                this.SerializeScrumTeam(team, stream);
            }
        }

        /// <summary>
        /// Deletes the Scrum team from repository.
        /// </summary>
        /// <param name="teamName">Name of the team.</param>
        public void DeleteScrumTeam(string teamName)
        {
            if (string.IsNullOrEmpty(teamName))
            {
                throw new ArgumentNullException("teamName");
            }

            string file = this.GetFileName(teamName);
            file = Path.Combine(this.Folder, file);

            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (IOException)
                {
                    // ignore, file might be already deleted
                }
            }
        }

        /// <summary>
        /// Deletes the expired Scrum teams, which were not used for period of expiration time.
        /// </summary>
        public void DeleteExpiredScrumTeams()
        {
            var expirationTime = this.dateTimeProvider.UtcNow - this.configuration.RepositoryTeamExpiration;
            var directory = new DirectoryInfo(this.Folder);
            if (directory.Exists)
            {
                var files = directory.GetFiles("*" + FileExtension);
                foreach (var file in files)
                {
                    try
                    {
                        if (file.LastWriteTimeUtc < expirationTime)
                        {
                            file.Delete();
                        }
                    }
                    catch (Exception)
                    {
                        // ignore and continue
                    }
                }
            }
        }

        /// <summary>
        /// Deletes all Scrum teams.
        /// </summary>
        public void DeleteAll()
        {
            var directory = new DirectoryInfo(this.Folder);
            if (directory.Exists)
            {
                var files = directory.GetFiles("*" + FileExtension);
                foreach (var file in files)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception)
                    {
                        // ignore and continue
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private static char[] GetInvalidCharacters()
        {
            var invalidFileCharacters = Path.GetInvalidFileNameChars();
            var result = new char[invalidFileCharacters.Length + 1];
            result[0] = SpecialCharacter;
            invalidFileCharacters.CopyTo(result, 1);
            Array.Sort<char>(result, Comparer<char>.Default);
            return result;
        }

        private string GetFolder()
        {
            return this.settings.Folder;
        }

        private void InitializeFolder()
        {
            if (!Directory.Exists(this.Folder))
            {
                Directory.CreateDirectory(this.Folder);
            }
        }

        private string GetFileName(string teamName)
        {
            var result = new StringBuilder(teamName.Length + 10);
            var invalidChars = this.invalidCharacters.Value;
            for (int i = 0; i < teamName.Length; i++)
            {
                char c = teamName[i];
                bool isSpecial = Array.BinarySearch<char>(invalidChars, c, Comparer<char>.Default) >= 0;
                isSpecial = isSpecial || (c != ' ' && char.IsWhiteSpace(c));

                if (isSpecial)
                {
                    result.Append(SpecialCharacter);
                    result.Append(((int)c).ToString("X4", CultureInfo.InvariantCulture));
                }
                else
                {
                    result.Append(c);
                }
            }

            result.Append(FileExtension);
            return result.ToString();
        }

        private string GetScrumTeamName(string filename)
        {
            var name = Path.GetFileNameWithoutExtension(filename);

            var result = new StringBuilder(name.Length);
            int specialPosition = 0;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (specialPosition == 0)
                {
                    if (c == SpecialCharacter)
                    {
                        if (name.Length <= i + 5)
                        {
                            return null;
                        }

                        int special = 0;
                        if (int.TryParse(name.Substring(i + 1, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out special))
                        {
                            result.Append((char)special);
                        }
                        else
                        {
                            return null;
                        }

                        specialPosition = 4;
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
                else
                {
                    specialPosition--;
                }
            }

            return result.ToString();
        }

        private void SerializeScrumTeam(ScrumTeam team, Stream stream)
        {
            var formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence));
            formatter.Serialize(stream, team);
        }

        private ScrumTeam DeserializeScrumTeam(Stream stream)
        {
            var formatter = this.dateTimeProvider != null ?
                new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence, this.dateTimeProvider)) :
                new BinaryFormatter();
            return (ScrumTeam)formatter.Deserialize(stream);
        }

        #endregion
    }
}
