<!DOCTYPE html>
<html lang="es">
<head>
<meta charset="UTF-8" />
<meta name="viewport" content="width=device-width, initial-scale=1.0" />
<title>Agente Oreo</title>
<link href="https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=JetBrains+Mono:wght@400;500&display=swap" rel="stylesheet" />
<style>
  *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

  :root {
    --bg:        #0e0e10;
    --surface:   #18181c;
    --surface2:  #222228;
    --border:    rgba(255,255,255,0.08);
    --accent:    #d4a843;
    --accent2:   #f0c96a;
    --text:      #f0ede8;
    --muted:     #8a8780;
    --user-bg:   #1e1c14;
    --user-bdr:  rgba(212,168,67,0.35);
    --code-bg:   #111113;
    --scrollbar: #2e2e36;
  }

  body {
    font-family: 'Syne', sans-serif;
    background: var(--bg);
    color: var(--text);
    height: 100vh;
    display: flex;
    flex-direction: column;
    overflow: hidden;
  }

  /* Header */
  header {
    display: flex;
    align-items: center;
    gap: 14px;
    padding: 18px 28px;
    border-bottom: 1px solid var(--border);
    background: var(--surface);
    flex-shrink: 0;
  }

  .oreo-logo {
    width: 42px; height: 42px;
    border-radius: 50%;
    background: linear-gradient(135deg, #1a1a1a 40%, #3a3a3a);
    border: 2px solid var(--accent);
    display: flex; align-items: center; justify-content: center;
    font-size: 20px;
    box-shadow: 0 0 16px rgba(212,168,67,0.25);
    animation: pulse 3s ease-in-out infinite;
    flex-shrink: 0;
  }

  @keyframes pulse {
    0%, 100% { box-shadow: 0 0 16px rgba(212,168,67,0.25); }
    50% { box-shadow: 0 0 28px rgba(212,168,67,0.5); }
  }

  .header-info h1 {
    font-size: 18px; font-weight: 800;
    letter-spacing: 0.04em;
    color: var(--accent);
  }
  .header-info p {
    font-size: 11px; color: var(--muted);
    font-family: 'JetBrains Mono', monospace;
    letter-spacing: 0.06em;
  }

  .status-dot {
    width: 8px; height: 8px; border-radius: 50%;
    background: #4ade80;
    margin-left: auto;
    box-shadow: 0 0 8px #4ade80;
    animation: blink 2s ease-in-out infinite;
  }
  @keyframes blink {
    0%,100% { opacity: 1; } 50% { opacity: 0.4; }
  }

  /* Chat area */
  #chat {
    flex: 1;
    overflow-y: auto;
    padding: 28px 24px;
    display: flex;
    flex-direction: column;
    gap: 20px;
    scroll-behavior: smooth;
  }

  #chat::-webkit-scrollbar { width: 5px; }
  #chat::-webkit-scrollbar-track { background: transparent; }
  #chat::-webkit-scrollbar-thumb { background: var(--scrollbar); border-radius: 99px; }

  /* Messages */
  .msg { display: flex; gap: 12px; max-width: 820px; animation: fadeUp 0.25s ease; }
  @keyframes fadeUp {
    from { opacity: 0; transform: translateY(8px); }
    to   { opacity: 1; transform: translateY(0); }
  }

  .msg.user { align-self: flex-end; flex-direction: row-reverse; }
  .msg.oreo  { align-self: flex-start; }

  .avatar {
    width: 34px; height: 34px; border-radius: 50%;
    display: flex; align-items: center; justify-content: center;
    font-size: 15px; flex-shrink: 0;
    margin-top: 2px;
  }
  .msg.oreo  .avatar { background: var(--surface2); border: 1.5px solid var(--accent); }
  .msg.user  .avatar { background: var(--user-bg);  border: 1.5px solid var(--user-bdr); font-size: 12px; font-weight: 700; color: var(--accent); }

  .bubble {
    padding: 12px 16px;
    border-radius: 14px;
    font-size: 15px;
    line-height: 1.7;
    max-width: 680px;
  }

  .msg.oreo .bubble {
    background: var(--surface);
    border: 1px solid var(--border);
    border-top-left-radius: 4px;
    color: var(--text);
  }
  .msg.user .bubble {
    background: var(--user-bg);
    border: 1px solid var(--user-bdr);
    border-top-right-radius: 4px;
    color: var(--accent2);
    font-weight: 500;
  }

  /* Code blocks inside bubbles */
  .bubble pre {
    background: var(--code-bg);
    border: 1px solid var(--border);
    border-radius: 8px;
    padding: 14px 16px;
    overflow-x: auto;
    margin: 10px 0 4px;
  }
  .bubble code {
    font-family: 'JetBrains Mono', monospace;
    font-size: 13px;
    color: #a8d5a2;
    white-space: pre;
  }
  .bubble p { margin-bottom: 8px; }
  .bubble p:last-child { margin-bottom: 0; }

  /* Thinking indicator */
  .thinking .bubble {
    display: flex; align-items: center; gap: 6px;
    color: var(--muted);
    font-size: 13px;
    font-family: 'JetBrains Mono', monospace;
  }
  .dot-anim { display: flex; gap: 4px; }
  .dot-anim span {
    width: 5px; height: 5px; border-radius: 50%;
    background: var(--accent);
    animation: bounce 1.2s ease-in-out infinite;
  }
  .dot-anim span:nth-child(2) { animation-delay: 0.2s; }
  .dot-anim span:nth-child(3) { animation-delay: 0.4s; }
  @keyframes bounce {
    0%,80%,100% { transform: translateY(0); opacity:0.4; }
    40% { transform: translateY(-5px); opacity:1; }
  }

  /* Welcome */
  .welcome {
    text-align: center;
    padding: 40px 20px;
    color: var(--muted);
    font-size: 13px;
    font-family: 'JetBrains Mono', monospace;
    letter-spacing: 0.04em;
  }
  .welcome strong { display: block; font-size: 28px; font-family: 'Syne', sans-serif; font-weight: 800; color: var(--accent); margin-bottom: 8px; letter-spacing: 0.06em; }

  /* Input bar */
  footer {
    padding: 16px 24px 20px;
    border-top: 1px solid var(--border);
    background: var(--surface);
    flex-shrink: 0;
  }

  .input-row {
    display: flex;
    gap: 10px;
    align-items: flex-end;
    background: var(--surface2);
    border: 1px solid var(--border);
    border-radius: 14px;
    padding: 10px 14px;
    transition: border-color 0.2s;
  }
  .input-row:focus-within { border-color: rgba(212,168,67,0.4); }

  #input {
    flex: 1;
    background: transparent;
    border: none;
    outline: none;
    color: var(--text);
    font-family: 'Syne', sans-serif;
    font-size: 15px;
    resize: none;
    max-height: 140px;
    min-height: 24px;
    line-height: 1.6;
    overflow-y: auto;
  }
  #input::placeholder { color: var(--muted); }

  #send-btn {
    width: 36px; height: 36px;
    border-radius: 50%;
    background: var(--accent);
    border: none;
    cursor: pointer;
    display: flex; align-items: center; justify-content: center;
    flex-shrink: 0;
    transition: transform 0.15s, background 0.15s;
  }
  #send-btn:hover  { background: var(--accent2); transform: scale(1.08); }
  #send-btn:active { transform: scale(0.95); }
  #send-btn svg { width: 16px; height: 16px; fill: #0e0e10; }

  .hint {
    font-size: 11px;
    color: var(--muted);
    font-family: 'JetBrains Mono', monospace;
    margin-top: 8px;
    text-align: center;
    letter-spacing: 0.04em;
  }

  /* API key warning */
  .api-warning {
    background: rgba(212,168,67,0.08);
    border: 1px solid rgba(212,168,67,0.25);
    border-radius: 10px;
    padding: 12px 16px;
    font-size: 13px;
    color: var(--accent2);
    font-family: 'JetBrains Mono', monospace;
    margin-bottom: 12px;
    display: flex;
    align-items: flex-start;
    gap: 10px;
  }
  .api-warning svg { flex-shrink:0; margin-top:1px; }

  /* Key input */
  #key-input {
    background: var(--code-bg);
    border: 1px solid rgba(212,168,67,0.3);
    border-radius: 8px;
    padding: 8px 12px;
    color: var(--accent2);
    font-family: 'JetBrains Mono', monospace;
    font-size: 12px;
    width: 100%;
    margin-top: 8px;
    outline: none;
  }
  #key-input:focus { border-color: var(--accent); }

  .key-save-btn {
    margin-top: 8px;
    background: var(--accent);
    color: #0e0e10;
    border: none;
    border-radius: 6px;
    padding: 6px 14px;
    font-family: 'Syne', sans-serif;
    font-weight: 700;
    font-size: 12px;
    cursor: pointer;
    transition: background 0.15s;
  }
  .key-save-btn:hover { background: var(--accent2); }
</style>
</head>
<body>

<header>
  <div class="oreo-logo">🍪</div>
  <div class="header-info">
    <h1>AGENTE OREO</h1>
    <p>llama-3.3-70b · groq api · c# expert</p>
  </div>
  <div class="status-dot" title="En línea"></div>
</header>

<div id="chat">
  <div class="welcome" id="welcome">
    <strong>OREO</strong>
    Tu asistente de programación en C#<br/>ingresa tu API Key de Groq para comenzar
  </div>
</div>

<footer>
  <div class="api-warning" id="key-section">
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
    <div style="width:100%">
      <div>Ingresa tu API Key de Groq (se guarda solo en esta sesión, nunca se envía a terceros)</div>
      <input id="key-input" type="password" placeholder="gsk_..." spellcheck="false" />
      <button class="key-save-btn" onclick="saveKey()">Guardar y comenzar</button>
    </div>
  </div>

  <div class="input-row">
    <textarea id="input" rows="1" placeholder="Escribe tu pregunta sobre C#..." disabled></textarea>
    <button id="send-btn" onclick="sendMessage()" title="Enviar">
      <svg viewBox="0 0 24 24"><path d="M2 21l21-9L2 3v7l15 2-15 2z"/></svg>
    </button>
  </div>
  <div class="hint">Enter para enviar &nbsp;·&nbsp; Shift+Enter para nueva línea</div>
</footer>

<script>
const MODELO  = "llama-3.3-70b-versatile";
const URL_API = "https://api.groq.com/openai/v1/chat/completions";
const SYSTEM  = "Tu nombre es Oreo. Eres un asistente experto en programación en C#. " +
                "Cuando te pregunten tu nombre, responde que te llamas Oreo. " +
                "Responde siempre en español, de forma clara y con ejemplos de código cuando sea necesario.";

let apiKey   = "";
let history  = [];
let thinking = false;

function saveKey() {
  const val = document.getElementById("key-input").value.trim();
  if (!val.startsWith("gsk_")) {
    alert("La API Key de Groq debe comenzar con 'gsk_'");
    return;
  }
  apiKey = val;
  document.getElementById("key-section").style.display = "none";
  document.getElementById("input").disabled = false;
  document.getElementById("input").focus();
  appendOreo("¡Hola! Soy **Oreo**, tu asistente de programación en C#. ¿En qué puedo ayudarte hoy?");
}

function appendUser(text) {
  const chat = document.getElementById("chat");
  document.getElementById("welcome")?.remove();
  const div = document.createElement("div");
  div.className = "msg user";
  div.innerHTML = `<div class="avatar">TÚ</div><div class="bubble">${escapeHtml(text)}</div>`;
  chat.appendChild(div);
  chat.scrollTop = chat.scrollHeight;
}

function appendOreo(text) {
  const chat = document.getElementById("chat");
  document.getElementById("welcome")?.remove();
  const div = document.createElement("div");
  div.className = "msg oreo";
  div.innerHTML = `<div class="avatar">🍪</div><div class="bubble">${formatText(text)}</div>`;
  chat.appendChild(div);
  chat.scrollTop = chat.scrollHeight;
  return div;
}

function showThinking() {
  const chat = document.getElementById("chat");
  const div  = document.createElement("div");
  div.className = "msg oreo thinking";
  div.id = "thinking-indicator";
  div.innerHTML = `<div class="avatar">🍪</div><div class="bubble"><span style="color:var(--muted);font-size:12px;font-family:'JetBrains Mono',monospace">oreo está pensando</span><div class="dot-anim"><span></span><span></span><span></span></div></div>`;
  chat.appendChild(div);
  chat.scrollTop = chat.scrollHeight;
}

function removeThinking() {
  document.getElementById("thinking-indicator")?.remove();
}

function escapeHtml(str) {
  return str.replace(/&/g,"&amp;").replace(/</g,"&lt;").replace(/>/g,"&gt;");
}

function formatText(text) {
  // code blocks
  text = text.replace(/```(\w*)\n?([\s\S]*?)```/g, (_, lang, code) =>
    `<pre><code>${escapeHtml(code.trim())}</code></pre>`);
  // inline code
  text = text.replace(/`([^`]+)`/g, '<code style="background:var(--code-bg);padding:2px 6px;border-radius:4px;font-size:13px;font-family:\'JetBrains Mono\',monospace;color:#a8d5a2">$1</code>');
  // bold
  text = text.replace(/\*\*(.+?)\*\*/g, '<strong style="color:var(--accent2)">$1</strong>');
  // newlines
  text = text.replace(/\n/g, "<br/>");
  return text;
}

async function sendMessage() {
  if (thinking || !apiKey) return;
  const input = document.getElementById("input");
  const text  = input.value.trim();
  if (!text) return;

  input.value = "";
  input.style.height = "auto";
  appendUser(text);
  history.push({ role: "user", content: text });

  thinking = true;
  showThinking();

  try {
    const res = await fetch(URL_API, {
      method: "POST",
      headers: {
        "Content-Type":  "application/json",
        "Authorization": `Bearer ${apiKey}`
      },
      body: JSON.stringify({
        model: MODELO,
        max_tokens: 1024,
        messages: [
          { role: "system", content: SYSTEM },
          ...history
        ]
      })
    });

    const data = await res.json();

    if (!res.ok) {
      removeThinking();
      appendOreo(`❌ Error ${res.status}: ${data.error?.message || JSON.stringify(data)}`);
      thinking = false;
      return;
    }

    const reply = data.choices[0].message.content;
    history.push({ role: "assistant", content: reply });
    removeThinking();
    appendOreo(reply);

  } catch (err) {
    removeThinking();
    appendOreo(`❌ Error de conexión: ${err.message}`);
  }

  thinking = false;
}

// Auto-resize textarea
const ta = document.getElementById("input");
ta.addEventListener("input", () => {
  ta.style.height = "auto";
  ta.style.height = Math.min(ta.scrollHeight, 140) + "px";
});

// Enter to send
ta.addEventListener("keydown", e => {
  if (e.key === "Enter" && !e.shiftKey) { e.preventDefault(); sendMessage(); }
});

// Enter on key input
document.getElementById("key-input").addEventListener("keydown", e => {
  if (e.key === "Enter") saveKey();
});
</script>
</body>
</html>
