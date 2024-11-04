# Notas do curso Azure para AZ-900 (certificado)

### intro
existe redes publicas e privadas (auto descritivo)

### indicadores financieros
- capex é referente a custos iniciais de ativos como hardware e fidicos
- opex é operacional com sistema,luz e etc

sendo ambiente azure um vm com uma interface bem separada, focar no necessario e segurançaç lembrando de fazer escolhas relevante a sua necessidade como lucalidade do sistema e tempo de uso de determinado sistema tem seus repectivos gastos no final do mes

## beneficios
- alta disponibilidade
  - por contrato se certo sistema ficar indisponivel por certo periodo de tempo recebe creditos caso a microsoft não entregue o prometido
  - conta com alertas e notificações para indentificar a disponibilidade de tal serviço
  - sla - Service Level Agreement – em português "Acordo de Nível de Serviço"
- escalabilidade e elasticidade
  - adaptar certos recursos com base em requisição, ex: se o consumo passar de 75% ele pode aumentar com base na sua config
- confiabilidade
  - a microsoft lida com o hardware e estabilidades dos sistemas
  - a implementação de segurança é por conta do usuario (qm config o azure) sendo eles oferecendo os recursos e nós configurando 
- governança
  - auditoria - qm vai poder mexer em q, gerir para cada usuario ter acesso somente ao que lhe for adequado
  - existem modelos para configurações de arquitetura

### SLA - Service Level Agreement
O SLA de atendimento é um documento que define as diretrizes operacionais, normas, procedimentos e métricas que devem ser seguidas por sua equipe de suporte, a fim de garantir o nível de satisfação dos clientes ideal.


TMP I - tempo de inatividade

|SLA|TMP I Semana|TMP I Mes|TMP I Ano|
|---|---|---|---|
|99%|1.68h|7.2h|3.65 dias|
|99,9%|10.1min|43.2min|8.76h|
|99,95%|5min|21.6min|4.38h|
|99,99%|1.01min|4.32min|52.56min|
|99,999%|6s|25.9s|5.26min|

#### !! essa tabela pode ser alterada, verificar nos portal oficial da azure microsoft !!

## Tipos de serviços

![Sou uma imagem](https://www.cimm.com.br/portal/uploads/cimm/asset/file/7447/large_aplicativos_hospedados.png)

- IaaS - infraestrutura como serviço
  - base inicial
- PaaS - plataforma como serviço
  - base media com SO e tool dev
- SaaS - software como serviço
  - base final que engloba tudo e inclui o app em sim
