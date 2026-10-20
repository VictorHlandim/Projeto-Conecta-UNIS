using System.Threading.Tasks;
using UnityEngine;

public class TodoList : MonoBehaviour
{
    // Start() can't be awaited, so kick off the async work from a Task method.
    private void Start()
    {
        _ = LoadTodosAsync();
    }

    private async Task LoadTodosAsync()
    {
        var manager = SupabaseManager.Instance;
        await manager.InitializationTask;

        var response = await SupabaseManager.Supabase.From<Todo>().Get();
        foreach (var todo in response.Models)
        {
            Debug.Log(todo.Name);
        }
    }
}