# foosball-ddd
A framework to track foosball games at the office, using Domain-Driven-Design as the architecture with Event Sourcing

## How to build
- Install [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)
- Clone the repository
- In project root, execute `dotnet build`

## Run the API
- `cd Foosball.WebApi`
- `dotnet run`
- Application host will be listening on http://localhost:5000 and https://localhost:5001
- Check swagger documentation for API end-points and try it out: http://localhost:5000/swagger
- Tested in macOS, Windows 10

### Foosball.Domain
`FoosballGame` is the **Aggregate Root**. 
Aggregate state is restored from the events store and it's the single source of truth. The state changes occurs in Domain (Writes) and gets published to interested parties via events. 

Events are persisted within a NoSQL document store using [LiteDB](https://www.litedb.org/). Tho, they could've been stored in a regular SQL database as well.

If the event store db/collection doesn't exist, it gets auto created. Document appears in the project root as _FoosballEventStore_. If you want to 'clear' the persisted data, just delete the document.

It utilizes a lightweight framework called [CQRSlite](https://github.com/gautema/CQRSlite) underneath to achieve event sourcing, restoring aggregate state, syncing changes, routing the events to consumers etc.

### Foosball.Projections
Projection data (Reads) gets built from the events happening in the domain and entites are stored in NoSQL document collection as well (_FoosballProjections_).
These 2 representations of the data are [eventually consistent](https://en.wikipedia.org/wiki/Eventual_consistency). 
When an API user queries the system and wants to read - the results are served from the projections.

### Foosball.Persistance
Provides interfaces, implementations for persisting data both for Projections and EventStore. 
Both has 2 concrete implementations. One with LiteDB, other with InMemory solution.
You can switch to in memory solution by changing the injected interfaces in [Startup.cs](https://github.com/ibo549/foosball-ddd/blob/main/Foosball.WebApi/Startup.cs)
