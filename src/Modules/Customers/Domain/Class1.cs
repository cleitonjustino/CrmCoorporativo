 using BuildingBlocks.Domain;
 using BuildingBlocks.SharedKernel;

 namespace Modules.Customers.Domain;

 public sealed class Cliente : AggregateRoot<ClienteId>
 {
     public Documento Documento { get; private set; } = default!;
     public Email Email { get; private set; } = default!;
     public Endereco? Endereco { get; private set; }
     public InscricaoEstadual? InscricaoEstadual { get; private set; }
     public DataNascimento? DataNascimento { get; private set; }
     public DataFundacaoEmpresa? DataFundacaoEmpresa { get; private set; }
     public bool EstaExcluido { get; private set; }

     public static Result<Cliente> Criar(
         ClienteId id,
         Documento documento,
         Email email,
         Endereco? endereco,
         InscricaoEstadual? inscricaoEstadual,
         DataNascimento? dataNascimento,
         DataFundacaoEmpresa? dataFundacaoEmpresa,
         IUnicidadeClienteService unicidade,
         MetadadosEvento metadados,
         DateTime agoraUtc,
         DateOnly hoje)
     {
         if (dataNascimento is not null)
         {
             if (dataNascimento.Value.ObterIdadeEm(hoje) < 18)
             {
                 return Result<Cliente>.Failure(new Error("cliente.idade_minima", "Pessoa Física deve ser maior ou igual a 18 anos."));
             }
         }

         if (dataFundacaoEmpresa is not null)
         {
             if (inscricaoEstadual is null)
             {
                 return Result<Cliente>.Failure(new Error("cliente.ie_obrigatoria", "Pessoa Jurídica deve informar IE ou marcar explicitamente como Isento."));
             }
         }

         if (dataNascimento is not null && dataFundacaoEmpresa is not null)
         {
             return Result<Cliente>.Failure(new Error("cliente.tipo_invalido", "Cliente não pode ser simultaneamente Pessoa Física e Pessoa Jurídica."));
         }

         if (!unicidade.DocumentoDisponivel(documento))
         {
             return Result<Cliente>.Failure(new Error("cliente.documento_duplicado", "Já existe um cliente com este CPF/CNPJ."));
         }

         if (!unicidade.EmailDisponivel(email))
         {
             return Result<Cliente>.Failure(new Error("cliente.email_duplicado", "Já existe um cliente com este e-mail."));
         }

         var cliente = new Cliente();
         cliente.Raise(new ClienteCriado(
             id,
             documento,
             email,
             endereco,
             inscricaoEstadual,
             dataNascimento,
             dataFundacaoEmpresa,
             metadados,
             agoraUtc));

         return Result<Cliente>.Success(cliente);
     }

     public Result AlterarEmail(Email novoEmail, IUnicidadeClienteService unicidade, MetadadosEvento metadados, DateTime agoraUtc)
     {
         if (EstaExcluido) return Result.Failure(new Error("cliente.excluido", "Cliente está excluído."));
         if (novoEmail.Value == Email.Value) return Result.Success();

         if (!unicidade.EmailDisponivelParaAlteracao(Id, Email, novoEmail))
         {
             return Result.Failure(new Error("cliente.email_duplicado", "Já existe um cliente com este e-mail."));
         }

         Raise(new EmailAlterado(Id, Email, novoEmail, metadados, agoraUtc));
         return Result.Success();
     }

     public Result AtualizarEndereco(Endereco novoEndereco, MetadadosEvento metadados, DateTime agoraUtc)
     {
         if (EstaExcluido) return Result.Failure(new Error("cliente.excluido", "Cliente está excluído."));

         Raise(new EnderecoAtualizado(Id, novoEndereco, metadados, agoraUtc));
         return Result.Success();
     }

     public Result Excluir(MetadadosEvento metadados, DateTime agoraUtc)
     {
         if (EstaExcluido) return Result.Success();

         Raise(new ClienteExcluido(Id, metadados, agoraUtc));
         return Result.Success();
     }

     protected override void Apply(IDomainEvent @event)
     {
         switch (@event)
         {
             case ClienteCriado e:
                 Id = e.ClienteId;
                 Documento = e.Documento;
                 Email = e.Email;
                 Endereco = e.Endereco;
                 InscricaoEstadual = e.InscricaoEstadual;
                 DataNascimento = e.DataNascimento;
                 DataFundacaoEmpresa = e.DataFundacaoEmpresa;
                 EstaExcluido = false;
                 break;
             case EmailAlterado e:
                 Email = e.NovoEmail;
                 break;
             case EnderecoAtualizado e:
                 Endereco = e.Endereco;
                 break;
             case ClienteExcluido:
                 EstaExcluido = true;
                 break;
         }
     }
 }
