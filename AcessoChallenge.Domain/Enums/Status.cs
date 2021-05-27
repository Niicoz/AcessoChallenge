using System.ComponentModel;

namespace AcessoChallenge.Domain.Enums
{
    public enum Status
    {
        [Description("In Queue")]
        InQueue = 1,

        Processing = 2,
        Confirmed = 3,
        Error = 4
    }
}