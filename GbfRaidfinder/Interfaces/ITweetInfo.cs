namespace GbfRaidfinder.Interfaces {
    public interface ITweetInfo {
        string User { get; set; }
        string Boss { get; set; }
        string Time { get; set; }
        string Id { get; set; }
        string Language { get; set; }
        bool Clicked { get; set; }
        bool Translated { get; set; }
        string Avatar { get; set; }
        string Text { get; set; }
    }
}