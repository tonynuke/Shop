const { exec } = require('child_process');

const clientGenCommandTemplate = (specificationPath, moduleName, clientName, className = "Client") => {
    const importString = "import { ProblemDetails } from '@/modules/common/problem-details'";
    const args = [
        "/template:Axios",
        "/excludedTypeNames:ProblemDetails",
        `/extensionCode:"${importString}"`,
        `/className:${className}`,
        `/input:${specificationPath}`,
        `/output:./src/modules/${moduleName}/client/${clientName}`
    ]

    const argsString = args.join(" ");
    return `nswag openapi2tsclient ${argsString}`;
};

const clientGenCommands = [
    clientGenCommandTemplate("../ApiGateways/ApiGateway.Client/OpenApi/v1.json", "api", "client.ts"),
    clientGenCommandTemplate("../Services/Identity/Identity.Client/OpenApi/v1.json", "identity", "client.ts", "IdentityClient"),
    clientGenCommandTemplate("../Services/Basket/Basket.Client/OpenApi/v1.json", "basket", "client.ts"),
    clientGenCommandTemplate("../Services/Catalog/Catalog.Client/OpenApi/v1.json", "catalog", "client.ts"),
];

console.log("Clients generation started");

clientGenCommands.map(command => {
    console.log(command);
    exec(command);
});

console.log("Clients generation finished");