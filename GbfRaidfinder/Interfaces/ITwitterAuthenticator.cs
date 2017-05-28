using System.Threading.Tasks;
using Tweetinvi.Models;

namespace GbfRaidfinder.Interfaces {
    public interface ITwitterAuthenticator {
        Task<bool> AuthenticateUser();
        void CreateAndSetCredentials(string pinCode);
        void SaveCredentials(ITwitterCredentials credentials);
    }
}