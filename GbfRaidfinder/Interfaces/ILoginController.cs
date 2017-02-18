using System.Threading.Tasks;
using Tweetinvi.Models;

namespace GbfRaidfinder.Interfaces {
    public interface ILoginController {
        string Pin { get; set; }
        Task<ITwitterCredentials> StartNewLogin();
    }
}