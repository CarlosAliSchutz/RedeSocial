import "./index.css";

export function Input({ label, name, value, onChange, ...props }) {
  return (
    <div className="input-wrapper">
      <label className="input-label">{label}</label>
      <input
        className="input"
        name={name}
        value={value}
        onChange={onChange}
        {...props}
      />
    </div>
  );
}
