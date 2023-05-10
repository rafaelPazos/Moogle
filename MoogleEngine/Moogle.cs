namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query)
    { 
        Data_Processing result = new Data_Processing(query);
        SearchItem[] items = new SearchItem[result.answer.Count];
        for (int i = 0; i < result.answer.Count; i++)
        {
            items[i] = new SearchItem(result.answer[i].FileName,result.answer[i].Snippet,result.answer[i].Score);
        }
        return new SearchResult(items, " ");
    }
}
