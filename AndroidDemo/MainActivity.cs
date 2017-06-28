using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Net.Http;

namespace AndroidDemo
{
    [Activity(Label = "AndroidDemo", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private TrelloItem item { set; get; } = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            ListView listView = FindViewById<ListView>(Resource.Id.listView1);
            Button button = FindViewById<Button>(Resource.Id.button1);

            button.Click += async delegate
            {
                button.Enabled = false;
                if (await getTrelloItem())
                    listView.SetAdapter(new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, item.cards));
                button.Enabled = true;
            };
        }

        private async Task<bool> getTrelloItem()
        {
            try
            {
                var json = await new HttpClient().GetStringAsync(
                        "https://api.trello.com/1/board/XUb9AMvz?key=ca4fcd3f80ad1dc2ff1667549030fe15&cards=open&lists=open");
                item = JsonConvert.DeserializeObject<TrelloItem>(json);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short);
                return false;
            }
            return true;
        }

    }
}

