namespace TrackSelection
{
    public class SelectedTrack {
    
        private static SelectedTrack instance;
        private int id;

        public string Name { get; private set; }

        protected SelectedTrack()
        {
            id = -1;
        }

        public static SelectedTrack GetInstance()
        {
            if (instance == null)
                instance = new SelectedTrack();
            return instance;
        }

        public void SetId(int selectedId)
        {
            id = selectedId;
        }

        public int GetId() {
            return id;
        }
    }
}