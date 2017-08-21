﻿using DPS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using DPS.Classes;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI.WebControls;
using System.Drawing;

namespace DPS
{
    public partial class cadastro_funcionario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!((bool)Session["Autenticado"]))
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                

                limparTela();

                string tipo = Request.QueryString["tipo"];
                tipo = tipo == null ? "CONSULTA" : tipo;
                Session["tipoAcao"] = tipo;
                if (tipo.Equals("EDITAR"))
                {
                    divconsulta.Visible = false;
                    divalteracao.Visible = true;

                    Session["cpffuncionario"] = Request.QueryString["cpf"];

                    string comando = @"SELECT * FROM Funcionarios WHERE cpf = '" + Session["cpffuncionario"].ToString() + "'";
                    DataTable data = Conexao.leitura(comando);
                    //Pessoal
                    txtCPF.Text = data.Rows[0]["cpf"].ToString();
                    txtCPF.Enabled = false;
                    txtNome.Text = data.Rows[0]["nome"].ToString();
                    txtNascimento.Text = data.Rows[0]["nascimento"].ToString();
                    //Endereço
                    txtCep.Text = data.Rows[0]["cep"].ToString();
                    txtEndereco.Text = data.Rows[0]["endereco"].ToString();
                    txtBairro.Text = data.Rows[0]["bairro"].ToString();
                    txtCidade.Text = data.Rows[0]["cidade"].ToString();
                    txtEstado.Text = data.Rows[0]["estado"].ToString();
                    //Contato
                    txtResidencial.Text = data.Rows[0]["telefone_residencial"].ToString();
                    txtComercial.Text = data.Rows[0]["telefone_comercial"].ToString();
                    txtCelular.Text = data.Rows[0]["telefone_celular"].ToString();
                    txtEmail.Text = data.Rows[0]["email"].ToString();
                }
                else
                {
                    if (tipo.Equals("INCLUIR"))
                    {
                        divconsulta.Visible = false;
                        divalteracao.Visible = true;
                    }
                    else
                    {
                        divconsulta.Visible = true;
                        divalteracao.Visible = false;
                    }
                }
            }
        }

        protected void limparTela()
        {

            txtCPF.Text = "";
            txtCPF.Enabled = true;
            txtNome.Text = "";
            txtNascimento.Text = "";
            //Endereço
            txtCep.Text = "";
            txtEndereco.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtEstado.Text = "";
            //Contato
            txtResidencial.Text = "";
            txtComercial.Text = "";
            txtCelular.Text = "";
            txtEmail.Text = "";
        }

        protected void lnkBtnPesquisar_Click(object sender, EventArgs e)
        {
            string filtro = "";
            
            if (!txtFiltroNome.Text.Equals(""))
            {
                filtro += " and nome like '%" + txtFiltroNome.Text + "%'";
            }

            string comando = @"SELECT * FROM Funcionarios WHERE ativo = 1 "+filtro;
            DataTable funcionarios = Conexao.leitura(comando);

            Session["Listafuncionarios"] = funcionarios;
            lblTotalResultados.Visible = true;

            if (funcionarios.Rows.Count > 0)
            {
                lblTotalResultados.Text = funcionarios.Rows.Count + " Resultados";
            }
            else
            {
                lblTotalResultados.Text = "Sem Resultados";

            }

            grid.DataSource = funcionarios;
            grid.DataBind();

        }

        protected void lnkBtnCadastrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("cadastro-funcionario.aspx?tipo=INCLUIR");
        }
        
        

        protected void linkExcluirfuncionarioVoltar_Click(object sender, EventArgs e)
        {
            divExcluir.Visible = false;
            genCortina.Visible = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("cadastro-financeiro-funcionario.aspx?tipo=CONSULTAR");
        }

        protected void btnSalvar_Click1(object sender, EventArgs e)
        {
            string cpf = txtCPF.Text.Replace(".", "").Replace("/", "").Replace("-", "");

            if (!Validacao.validaCPF(cpf))
            {
                MessageBox.Show("O CPF é Inválido!");
                return;
            }

            if(txtEmail.Text.Equals(""))
            {
                MessageBox.Show("O Email não pode ser vazio!");
                return;
            }

            DateTime nasc = DateTime.Now;
            try
            {
                nasc = Convert.ToDateTime(txtNascimento.Text);
                if(nasc > DateTime.Now.AddYears(-18))
                {
                    MessageBox.Show("Funcionário menor de idade não permitido!");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Data inválida!");
                return;
            }
            
            if (Session["tipoAcao"].ToString().Equals("EDITAR"))
            {
                string comando = @"update funcionarios set nome='{0}', nascimento='{1}', cep='{2}', endereco='{3}', bairro='{4}', cidade='{5}', estado='{6}', telefone_residencial='{7}', telefone_comercial='{8}', telefone_celular='{9}', email='{10}'
                                                where cpf='{11}'";
                comando = string.Format(comando,
                                            txtNome.Text,
                                            nasc.ToString("yyyy-MM-dd"),
                                            txtCep.Text.Replace(".", "").Replace("/", "").Replace("-", ""),
                                            txtEndereco.Text,
                                            txtBairro.Text,
                                            txtCidade.Text,
                                            txtEstado.Text,
                                            txtResidencial.Text,
                                            txtComercial.Text,
                                            txtCelular.Text,
                                            txtEmail.Text,
                                            cpf);

                Conexao.escrita(comando);

                MessageBox.Show("Alteração Realizada Com Sucesso!");
                Response.Redirect("cadastro-funcionario.aspx?tipo=CONSULTAR");       
            }
            else
            {
                string senha = Senha.gerarSenha();

                try
                {
                    string comando = @"insert into funcionarios (cpf, nome, nascimento, cep, endereco, bairro, cidade, estado, telefone_residencial, telefone_comercial, telefone_celular, email, ativo,senha) values
                                        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',1,'{12}')";
                    comando = string.Format(comando,
                                                cpf,
                                                txtNome.Text,
                                                nasc.ToString("yyyy-MM-dd"),
                                                txtCep.Text.Replace(".", "").Replace("/", "").Replace("-", ""),
                                                txtEndereco.Text,
                                                txtBairro.Text,
                                                txtCidade.Text,
                                                txtEstado.Text,
                                                txtResidencial.Text,
                                                txtComercial.Text,
                                                txtCelular.Text,
                                                txtEmail.Text,
                                                Senha.CalculateMD5Hash(senha));

                    Conexao.escrita(comando);
                }
                catch
                {
                    MessageBox.Show("Funcionário Não Cadastrado Favor Verificar os dados!");
                    return;
                }

                try
                {
                    string titulo = "Senha do Novo Funcionário";
                    string mensagem = @" Sua senha de acesso ao sistema é: " + senha + " guarde em segurança";
                    Email.enviaEmail(txtEmail.Text, titulo, mensagem);
                }
                catch
                {
                    MessageBox.Show("Não Foi possível enviar o Email com a Senha!");
                    return;
                }
            

                MessageBox.Show("Cadastro Realizado Com Sucesso!");
                Response.Redirect("cadastro-funcionario.aspx?tipo=CONSULTAR");
            }
        }
        
        protected void lnkBtnRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;

                if (grid.AutoGenerateColumns)
                {
                    tbl = grid.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = grid.Columns.Count-2;
                    noOfRows = grid.Rows.Count;
                }



                float HeaderTextSize = 8;
                float ReportNameSize = 10;
                float ReportTextSize = 8;
                float ApplicationNameSize = 7;

                // Creates a PDF document

                Document document = null;
             
                    document = new Document(PageSize.A4, 0, 0, 15, 5);
                

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(2);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase("Cadastro dos Funcionarios", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_LEFT;

                // Creates a phrase to show the current date at the right hand side of the header.
                Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                // Creates a PdfPCell which accepts the date phrase as a parameter.
                PdfPCell clDate = new PdfPCell(phDate);
                // Sets the Horizontal Alignment of the PdfPCell to right.
                clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                // Sets the border of the cell to zero.
                clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                // Creates a phrase which holds the file name.
                Phrase phHeader = new Phrase("Funcionarios Cadastrados", FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                mainTable.AddCell(clHeader);

                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (grid.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(grid.Columns[i].HeaderText, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }

                    mainTable.AddCell(ph);
                }

                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo < noOfRows; rowNo++)
                {
                    for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                    {
                        if (grid.AutoGenerateColumns)
                        {
                            string s = grid.Rows[rowNo].Cells[columnNo].Text.Trim();
                            if (s.Equals("&nbsp;"))
                                s = "";
                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                            mainTable.AddCell(ph);
                        }
                        else
                        {
                            if (grid.Columns[columnNo] is TemplateField)
                            {
                                DataBoundLiteralControl lc = grid.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                string s = lc.Text.Trim();
                                if (s.Equals("&nbsp;"))
                                    s = "";
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                mainTable.AddCell(ph);
                            }
                            else
                            {
                                string s = grid.Rows[rowNo].Cells[columnNo].Text.Trim();
                                if (s.Equals("&nbsp;"))
                                    s = "";
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                mainTable.AddCell(ph);
                            }
                        }
                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                PdfWriter.GetInstance(document, Response.OutputStream);

                //// Creates a footer for the PDF document.
                //HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
                //pdfFooter.Alignment = Element.ALIGN_CENTER;
                //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //// Sets the document footer to pdfFooter.
                //document.Footer = pdfFooter;
                // Opens the document.
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=Relatorio de Funcionarios de "+DateTime.Now.ToLongDateString()+".pdf");
                Response.End();
            }
            catch { }
        }

        protected void btnBuscaCEP_Click(object sender, EventArgs e)
        {
            Endereco end = new Endereco().GetByCEP(txtCep.Text);
            
            if (!string.IsNullOrEmpty(end.logradouro))
            {
                txtEndereco.Text = end.logradouro.ToUpper();
                txtBairro.Text = end.bairro.ToUpper();
                txtEstado.Text = end.uf.Trim().ToUpper();
                txtCidade.Text = end.cidade.ToUpper();
            }
            else
            {
                MessageBox.Show("CEP não encontrado!");
            }
        }

        protected void grid_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            DataTable funcionarios = (DataTable)Session["Listafuncionarios"];

            if (e.CommandName == "Excluir")
            {
                divExcluir.Visible = true;
                genCortina.Visible = true;
                lblExcluir.Text = funcionarios.Rows[index]["nome"].ToString();
                lblCpf.Text = funcionarios.Rows[index]["cpf"].ToString();
            }
            else
            {
                Session["cpffuncionario"] = funcionarios.Rows[index]["cpf"].ToString();
                Response.Redirect("cadastro-funcionario.aspx?tipo=EDITAR&cpf=" + Session["cpffuncionario"].ToString());
            }
        }

        protected void linkExcluirConfirme_Click(object sender, EventArgs e)
        {
            string comando = "UPDATE funcionarios SET ativo = 0 where cpf = '"+lblCpf.Text+"'";
            Conexao.escrita(comando);

            divExcluir.Visible = false;
            genCortina.Visible = false;
            MessageBox.Show("Excluído com Sucesso!");
            lnkBtnPesquisar_Click(null, null);
        }
    }
}