namespace DevExpressGrid3.ViewModels
{
    public class IndexViewModel
    {

        public string Parenttableid { get; set; }
        //public string Contractstatus { get; set; }

        public IndexViewModel(string parenttableid)
        {
            Parenttableid = parenttableid;
            //Contractstatus = contractstatus;
        }
    }
}