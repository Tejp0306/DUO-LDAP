using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DUO_LDAP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // string ldapServer = "ldap://your-ldap-server.com";  // LDAP server URL
             string ldapServer = "ldap://192.168.1.10";
            string baseDn = "dc=osumc.edu,dc=com"; // Base DN for your search
            string ldapUser = "cn=honetest,dc=osumc.edu,dc=com"; // Bind DN (your user in LDAP)
            string ldapPassword = "blurry-Tr33!"; // Password for the bind user

            try
            {
                LdapConnection connection = new LdapConnection(new LdapDirectoryIdentifier(ldapServer));
                connection.Credential = new NetworkCredential(ldapUser, ldapPassword);
                connection.AuthType = AuthType.Basic;

                // Bind to the LDAP server
                connection.Bind();

                Console.WriteLine("LDAP connection successful.");

                // Perform a search to retrieve all users
                SearchRequest searchRequest = new SearchRequest(
                    baseDn,  // Base DN to search from
                    "(objectClass=user)",  // Search filter to find all user objects
                    SearchScope.Subtree,  // Search the entire directory tree
                    new string[] { "cn", "sAMAccountName", "mail" } // Attributes to retrieve
                );

                // Send the request and get the response
                SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

                // Iterate through all the entries and print the results
                foreach (SearchResultEntry entry in searchResponse.Entries)
                {
                    Console.WriteLine($"CN: {entry.Attributes["cn"][0]}");
                    Console.WriteLine($"Username (sAMAccountName): {entry.Attributes["sAMAccountName"][0]}");
                    Console.WriteLine($"Email: {entry.Attributes["mail"][0]}");
                    Console.WriteLine();
                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine("Error connecting to LDAP server: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

   
    }
}
