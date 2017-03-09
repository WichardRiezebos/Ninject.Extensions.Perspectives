using Ninject.Extensions.Perspectives.Perspectives;

namespace Ninject.Extensions.Perspectives.Cases
{
    public class Repo
    {
        private readonly IPath path;

        public Repo(IPath path)
        {
            this.path = path;
        }

        public bool DoesRandomWork() => !string.IsNullOrEmpty(path.GetRandomFileName());
    }
}
