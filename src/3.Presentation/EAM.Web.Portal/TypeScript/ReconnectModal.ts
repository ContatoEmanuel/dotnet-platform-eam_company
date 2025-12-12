/**
 * Blazor Reconnection Modal Handler
 * Manages SignalR circuit reconnection UI for Blazor Server
 */

// Extend Window interface to include Blazor
declare global {
    interface Window {
        Blazor: {
            reconnect(): Promise<boolean>;
            resumeCircuit(): Promise<boolean>;
        };
    }
}

interface ReconnectStateChangedEvent extends CustomEvent {
    detail: {
        state: 'show' | 'hide' | 'failed' | 'rejected';
    };
}

// Get modal and button elements
const reconnectModal = document.getElementById("components-reconnect-modal") as HTMLDialogElement;
const retryButton = document.getElementById("components-reconnect-button") as HTMLButtonElement;
const resumeButton = document.getElementById("components-resume-button") as HTMLButtonElement;

// Set up event handlers
reconnectModal.addEventListener("components-reconnect-state-changed", handleReconnectStateChanged as EventListener);
retryButton.addEventListener("click", retry);
resumeButton.addEventListener("click", resume);

/**
 * Handles reconnection state changes from Blazor
 */
function handleReconnectStateChanged(event: ReconnectStateChangedEvent): void {
    const state = event.detail.state;
    
    if (state === "show") {
        reconnectModal.showModal();
    } else if (state === "hide") {
        reconnectModal.close();
    } else if (state === "failed") {
        document.addEventListener("visibilitychange", retryWhenDocumentBecomesVisible);
    } else if (state === "rejected") {
        location.reload();
    }
}

/**
 * Attempts to reconnect to the Blazor circuit
 */
async function retry(): Promise<void> {
    document.removeEventListener("visibilitychange", retryWhenDocumentBecomesVisible);

    try {
        // Reconnect will asynchronously return:
        // - true to mean success
        // - false to mean we reached the server, but it rejected the connection (e.g., unknown circuit ID)
        // - exception to mean we didn't reach the server (this can be sync or async)
        const successful = await window.Blazor.reconnect();
        
        if (!successful) {
            // We have been able to reach the server, but the circuit is no longer available.
            // We'll reload the page so the user can continue using the app as quickly as possible.
            const resumeSuccessful = await window.Blazor.resumeCircuit();
            
            if (!resumeSuccessful) {
                location.reload();
            } else {
                reconnectModal.close();
            }
        }
    } catch (err) {
        // We got an exception, server is currently unavailable
        console.error('Reconnection failed:', err);
        document.addEventListener("visibilitychange", retryWhenDocumentBecomesVisible);
    }
}

/**
 * Attempts to resume the Blazor circuit
 */
async function resume(): Promise<void> {
    try {
        const successful = await window.Blazor.resumeCircuit();
        
        if (!successful) {
            location.reload();
        }
    } catch (err) {
        console.error('Resume circuit failed:', err);
        location.reload();
    }
}

/**
 * Retries connection when document becomes visible
 */
async function retryWhenDocumentBecomesVisible(): Promise<void> {
    if (document.visibilityState === "visible") {
        await retry();
    }
}

export {};
