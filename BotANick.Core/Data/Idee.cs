using System;
using System.Collections.Generic;
using System.Text;
using BotANick.Core.Data.Constantes;

namespace BotANick.Core.Data
{
    public class Idee
    {
        private bool _hasBeenModified = false;

        /// <summary>
        /// L'identifiant de l'idée.
        /// </summary>
        public int IdeeId { get; set; }

        /// <summary>
        /// La description de l'idée.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Un commentaire supplémentaire de l'idée.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// La date de création de l'idée.
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// L'état de l'idée.
        /// </summary>
        public EtatsIdees EtatIdee { get; set; }

        /// <summary>
        /// Le nombre de votes pour l'idée.
        /// </summary>
        public int NombreVotes { get; set; }

        /// <summary>
        /// Le nom du créateur de l'idée.
        /// </summary>
        public string Createur { get; set; }

        /// <summary>
        /// Indique si l'idée est archivée.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// L'identifiant du message discord de l'idée.
        /// </summary>
        public ulong? IdMsgDiscord { get; set; }

        public bool IsModified()
        {
            return _hasBeenModified;
        }

        public void Modify()
        {
            _hasBeenModified = true;
        }
    }
}
