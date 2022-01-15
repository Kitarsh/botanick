namespace BotANick.Data.Constants
{
    /// <summary>
    /// Décrit les différents états d'une idée.
    /// </summary>
    public enum EtatsIdees
    {
        /// <summary>
        /// L'idée est soumise.
        /// </summary>
        Soumise = 1,

        /// <summary>
        /// L'idée est en cours de réalisation.
        /// </summary>
        EnCours = 2,

        /// <summary>
        /// L'idée a été terminée.
        /// </summary>
        Faite = 3,

        /// <summary>
        /// L'idée a été rejetée.
        /// </summary>
        Rejetee = 4,
    }
}
