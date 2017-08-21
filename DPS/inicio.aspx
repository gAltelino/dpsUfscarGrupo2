<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inicio.aspx.cs" Inherits="DPS.inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
 <head>
    <title>DPS - Grupo 2</title>
    <meta charset="UTF-8" />
    <meta name="description" content="Wikipedia Viewer FCC" />
    <meta name="keywords" content="Free Code Camp, Wikipedia" />
    <meta name="author" content="Giovani Altelino" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="boot4Beta/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="style.css" />
</head>
 
<body>
    <div class="container-fluid"></div>
       
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
            <a class="navbar-brand" href="#" id="mapaLogo">DPS</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
              <span class="navbar-toggler-icon"></span>
            </button>
          
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
              <ul class="navbar-nav mr-auto">
                <li class="nav-item">
                  <a class="nav-link" href="#" id="emergencia">Emergência</a>
                          </li>
                <li class="nav-item">
                  <a class="nav-link" href="#" id="mapa">Mapa</a>
                </li>
                <li class="nav-item">
                  <a class="nav-link" href="cadastro-funcionario.aspx" target="corpo" id="cadastroFuncionario">Cadastro de Funcionário</a>
                </li>
                <li class="nav-item">
                  <a class="nav-link" href="#" id="cadastroCliente">Cadastro de Cliente</a>
                </li>
                <li class="nav-item">
                        <a class="nav-link" href="#" id="cadastroRonda">Cadastro de Ronda</a>
                </li>
                <li class="nav-item">
                        <a class="nav-link" href="#" id="novaOcorrencia">Nova Ocorrência</a>
                </li>
                <li class="nav-item">
                        <a class="nav-link" href="#" id="historicoOcorrencia">Histórico de Ocorrência</a>
                </li>
                <li class="nav-item">
                        <a class="nav-link" href="#" id="historicoRota">Histórico de Rotas</a>
                </li>
              </ul>
             </div>
          </nav>
    
    
    <iframe id="corpo" name="corpo" src="" width="100%" height="2000px" onscroll="no" style="border:none;">

    </iframe>   

</body>
 
    <script src="jquery-3.2.1.min.js"></script>
    <script src="functions.js"></script>
    <script src="boot4Beta/js/bootstrap.min.js"></script>
</html>
