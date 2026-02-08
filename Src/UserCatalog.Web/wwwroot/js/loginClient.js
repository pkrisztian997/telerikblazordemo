export async function postJson(url, data) {
    const resp = await fetch(url, {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    if (!resp.ok) {
        const text = await resp.text();
        throw new Error(text || resp.statusText);
    }

    return await resp.text();
}
