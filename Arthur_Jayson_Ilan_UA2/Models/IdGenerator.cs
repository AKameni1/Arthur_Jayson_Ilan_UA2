using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public static class IdGenerator
    {
        private static int _currentUserId = 0;
        private static int _currentTicketId = 0;
        private static readonly object _lock = new object();

        /// <summary>
        /// Initialise le générateur avec le dernier identifiant utilisé.
        /// </summary>
        /// <param name="lastUserId">Le dernier identifiant d'utilisateur utilisé.</param>
        public static void Initialize(int lastUserId)
        {
            lock (_lock)
            {
                if (lastUserId >= _currentUserId)
                {
                    _currentUserId = lastUserId;
                }
            }
        }

        /// <summary>
        /// Génère un nouvel identifiant d'utilisateur unique.
        /// </summary>
        /// <returns>Un identifiant d'utilisateur unique.</returns>
        public static int GetNextUserId()
        {
            lock (_lock)
            {
                _currentUserId++;
                return _currentUserId;
            }
        }

        /// <summary>
        /// Initialise le générateur avec le dernier identifiant de ticket utilisé.
        /// </summary>
        /// <param name="lastTicketId">Le dernier identifiant de ticket utilisé.</param>
        public static void InitializeTicketId(int lastTicketId)
        {
            lock (_lock)
            {
                if (lastTicketId >= _currentTicketId)
                {
                    _currentTicketId = lastTicketId;
                }
            }
        }

        /// <summary>
        /// Génère un nouvel identifiant de ticket unique.
        /// </summary>
        /// <returns>Un identifiant de ticket unique.</returns>
        public static int GetNextTicketId()
        {
            lock (_lock)
            {
                _currentTicketId++;
                return _currentTicketId;
            }
        }
    }
}
