/**
 * Blazor Reconnection Modal Handler
 * Manages SignalR circuit reconnection UI for Blazor Server
 */
declare global {
    interface Window {
        Blazor: {
            reconnect(): Promise<boolean>;
            resumeCircuit(): Promise<boolean>;
        };
    }
}
export {};
//# sourceMappingURL=ReconnectModal.d.ts.map