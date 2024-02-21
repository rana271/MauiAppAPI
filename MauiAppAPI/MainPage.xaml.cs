using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppAPI;

public class Post 
{
    string _name;
	public int Id { get; set; }
	public string Title {
        get; set;
    }
	public string Body { get; set; }
    

}
public partial class MainPage : ContentPage
{

	private const string apiURl= "https://jsonplaceholder.typicode.com/posts";
	private HttpClient _client=new HttpClient();
    private ObservableCollection<Post> _post;
    public MainPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
       
        var content = await _client.GetStringAsync(apiURl);
        var posts = JsonConvert.DeserializeObject<List<Post>>(content);

        _post= new ObservableCollection<Post>(posts);
        postListView.ItemsSource = _post;
        base.OnAppearing();
    }

    async private void Button_Clicked(object sender, EventArgs e)
    {
        var post = new Post { Title = "Title" + DateTime.Now.Ticks };
        _post.Insert(0, post);
        var content=JsonConvert.SerializeObject(post);
        await _client.PostAsync(apiURl,new StringContent(content));
     
    }

    async private void Button_Clicked_1(object sender, EventArgs e)
    {
        var post = _post[0];
        post.Title += "updated";
        var content = JsonConvert.SerializeObject(post);
        await _client.PutAsync(apiURl +"/"+ post.Id,new StringContent(content));
        
    }

    async private void Button_Clicked_2(object sender, EventArgs e)
    {
        var post = _post[0];
        
        await _client.DeleteAsync(apiURl + "/" + post.Id);
        _post.Remove(post);
    }
}

