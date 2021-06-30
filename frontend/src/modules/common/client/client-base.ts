// https://github.com/RicoSuter/NSwag/issues/2743
export class ClientBase {

    public token?: string;

    constructor() {
        this.token = undefined;
    }

    protected transformOptions(options: any) {
        if (this.token) {
            options.headers["Authorization"] = "Bearer " + this.token;
        } else {
            console.warn("Authorization token has not been set, please authorize first.");
        }
        return Promise.resolve(options);
    }
}