
namespace Graffiti.Core
{
    public class Permission {
        private bool _read;
        private bool _edit;
        private bool _publish;

        public bool Read {
            get { return _read; }
            set { _read = value; }
        }

        public bool Edit {
            get { return _edit; }
            set { _edit = value; }
        }

        public bool Publish {
            get { return _publish; }
            set { _publish = value; }
        }
    }
}
