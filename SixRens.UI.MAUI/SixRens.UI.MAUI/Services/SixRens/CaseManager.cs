using SixRens.Core.占例存取;

namespace SixRens.UI.MAUI.Services.SixRens
{
    public sealed class CaseManager
    {
        private readonly DataStorager storager;
        internal CaseManager(DataStorager storager)
        {
            this.storager = storager;
        }

        public IEnumerable<(Guid id, string name, 占例 content)> ListCases()
        {
            foreach (var (id, name, content) in storager.ListCases())
            {
                yield return (id, name, 占例.反序列化(content));
            }
        }
        public Guid CreateCase(string name, 占例 content)
        {
            return storager.CreateCase(name, content.序列化());
        }
        public void UpdateCase(Guid id, string name, 占例 content)
        {
            storager.UpdateCase(id, name, content.序列化());
        }
        public void RemoveCase(Guid id)
        {
            storager.RemoveCase(id);
        }
    }
}
