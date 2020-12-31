using System;
using System.Collections.Generic;
using System.Text;
using BotANick.Data.Constantes;

namespace BotANick.Data
{
    public class Idee
    {
        /// <summary>
        /// L'identifiant de l'idée.
        /// </summary>
        public int IdeeId { get; set; }

        /// <summary>
        /// La description de l'idée.
        /// </summary>
        public string Description { get; set; }

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
        /// L'identifiant du message discord de l'idée.
        /// </summary>
        public ulong? IdMsgDiscord { get; set; }
    }
}
