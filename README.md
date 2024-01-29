# Teste técnico - konsi

A API foi produzida com o .NET 8.0 e utilizando frameworks bem conhecidos.
Seguindo a ideia solicitada, parti de uma solução bem simples, com poucas dependencias e projetos acoplados. Apesar de muitos designs poderem ser aplicados aqui, a minha opinião é que cada cenário requer uma abstração que valha sua complexidade. O desafio apesar de requerer algumas conexões e provisionamentos de infra, não criou a necessidade de criar-se vários projetos para continuar tendo uma boa visibilidade e organização.

Na camada de apresentação (controladores), optei por deixar duas entradas, uma que irá disparar a mensagem para o message broker e então buscará o dado no serviço externo e consolidará no nosso Cache e Elastic. Apesar de não ter sido requisitado um endpoint para isso, achei mais confortável para testes.
O outro endpoint segue o solicitado, busca via Elastic o dado que está na nossa esfera.

Alguns pontos da aplicação podem e devem ser melhorados para segurança e tendem a ser centralizados, como por exemplo as credenciais utilizadas no sistema.

Para melhora de testes, toda a aplicação e suas dependências estão configuradas em um Docker Compose.

### Para executar a aplicação, execute o código no projeto "konsi-api":
```bash
  docker-compose up -d
```

A API, Message broker, Redis e ElasticSearch estarão disponíveis e conectados após isso.

Para fazer o input dos CPFs(Enviar o evento que busca esses dados e atualiza nossa base) utilize a rota [POST] /Benefits, que estará no Swagger.

Para consumir os dados, utilize a chamada [GET] do mesmo controlador.
