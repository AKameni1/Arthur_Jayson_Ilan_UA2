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

        // Si vous avez d'autres modèles nécessitant des identifiants uniques,
        // vous pouvez ajouter des compteurs séparés pour chacun.
    }
}
