using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp
{
    /// <summary>
    /// Informacje o sesji. Przechowujemy tutaj ID użytkownika oraz jego imię celem kontekstu do dalszej integracji z programem.
    /// </summary>
    public static class Session
    {
        public static int userID = -1;
        public static string userFirstName = "";
    }
}
