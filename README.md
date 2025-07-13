# YarpGatewayWithRedisThrottle

This project demonstrates setting up an API Gateway using YARP and Redis for throttling in a .NET 8 environment. It includes two backend clients and a gateway that routes requests and applies throttling.

## Features

- API Gateway built with YARP (.NET 8)
- Redis-based request throttling
- Two backend clients (Client1 and Client2)
- Clients communicate through the gateway

## Project Structure

    /YarpGatewayWithRedisThrottle
    ├── Gateway/            # API Gateway running on https://localhost:7214
    ├── Client1/            # Backend Client1 running on https://localhost:7050
    ├── Client2/            # Backend Client2 running on https://localhost:7091
    └── README.md

## Prerequisites

- .NET 8 SDK installed
- Docker installed (used to run Redis)

## Running Redis

Run Redis locally with Docker:

    docker run -d --name redis-rate-limit -p 6379:6379 redis:alpine

This command will pull the Redis image if needed, start Redis detached, and expose it on port 6379.

Make sure Redis is running before starting the gateway.

## Running the Projects

Run the projects using Visual Studio or CLI:

- Gateway: `https://localhost:7214`
- Client1: `https://localhost:7050`
- Client2: `https://localhost:7091`

## Routing and Endpoints

- Client1 exposes endpoint: `GET /api/Client1/test`
- Client2 exposes endpoint: `GET /api/Client2/test`

Each client can call the other via the Gateway:

- Client1 calls `https://localhost:7214/api/Client2/test` via injected HttpClient configured with Gateway URL.
- Client2 calls `https://localhost:7214/api/Client1/test` similarly.

The gateway routes requests to correct backend based on URL path.

## Gateway Configuration (appsettings.json snippet)

    "Routes": [
      {
        "RouteId": "client1",
        "ClusterId": "client1",
        "Match": {
          "Path": "/api/Client1/{**catch-all}"
        }
      },
      {
        "RouteId": "client2",
        "ClusterId": "client2",
        "Match": {
          "Path": "/api/Client2/{**catch-all}"
        }
      }
    ],
    "Clusters": {
      "client1": {
        "Destinations": {
          "client1/destination": {
            "Address": "https://localhost:7050/"
          }
        }
      },
      "client2": {
        "Destinations": {
          "client2/destination": {
            "Address": "https://localhost:7091/"
          }
        }
      }
    }

## Throttling

- Gateway uses Redis to store request counts and enforce throttling.
- Requests over limit return `429 Too Many Requests`.
- Throttling is implemented as middleware in the Gateway.

## Requirements

- .NET 8 SDK
- Docker (for Redis)
- Visual Studio 2022 or CLI

## License

Provided as example code for educational purposes without specific license.
