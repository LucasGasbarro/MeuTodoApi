using System.ComponentModel.DataAnnotations;

namespace MeuTodo.ViewModels
{
    public class ModifyTodoViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
