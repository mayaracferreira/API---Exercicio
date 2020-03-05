using API.Data;
using API.Migrations;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace API.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    
    {

        private readonly ApplicationDbContext database;

        public ProdutoController (ApplicationDbContext database){
            this.database = database;
        }

        [HttpGet]
        public IActionResult Get() {
            var produtos = database.Produtos.ToList ();
                return Ok (produtos); //Statys Code = 200 && dados
        }

        [HttpGet("{Id}")]
        public IActionResult Get (int Id){
            try{
                var produto = database.Produtos.First(p => p.Id == Id);
                return Ok (produto);
            }catch (Exception ) {
                Response.StatusCode = 404;
                return new ObjectResult (new{msg = "Id inválido"});

            }
        
        }

        [HttpPost]
        public IActionResult Post ([FromBody] ProdutoTemp pTemp){

            /*Validação*/

            if(pTemp.Preco <= 0){
                Response.StatusCode = 400;
                return new ObjectResult (new {msg = "Preço do produto não pode ser menor ou igual à 0."});
            }

            if(pTemp.Nome.Length <= 1) {
                Response.StatusCode = 400;
                return new ObjectResult (new{msg = "O nome do Produto precisa ter mais de um caracter."});
            }

            Produto p = new Produto ();
            p.Nome = pTemp.Nome;
            p.Preco = pTemp.Preco;
            database.Produtos.Add(p);
            database.SaveChanges ();

            Response.StatusCode = 201;
            return new ObjectResult (new {msg = "Produto criado com sucesso!"});
            //return Ok (new {msg = "Produto criado com sucesso!"});

        }

        [HttpDelete("{Id}")]
        public IActionResult Delete (int Id){
               try{
                var produto = database.Produtos.First(p => p.Id == Id);
                database.Produtos.Remove(produto);
                database.SaveChanges();
                return Ok ();
            
            }catch (Exception ) {
                Response.StatusCode = 404;
                return new ObjectResult (new{msg = "Id inválido"});

            }
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] Produto produto){
            if (produto.Id > 0) {

                try{ 
                var p = database.Produtos.First(pTemp => pTemp.Id == produto.Id);
                
                if(p != null){

                    //editar
                p.Nome = produto.Nome != null ? produto.Nome : p.Nome;
                p.Preco = produto.Preco != 0 ? produto.Preco : p.Preco;

                    database.SaveChanges ();
                    return Ok ();

                }
                else{
                Response.StatusCode = 404;
                return new ObjectResult (new{msg = "Produto não encontrado"});
                }

                }catch {
                Response.StatusCode = 400;
                return new ObjectResult (new{msg = "Produto não encontrado"});
                }


            }else{
                Response.StatusCode = 400;
                return new ObjectResult (new{msg = "Id do produto inválido"});
            }

        }


    }

        //SWAGGER

        public class ProdutoTemp {

            public string Nome {get;set;}

            public float Preco {get;set;}
        }

}
