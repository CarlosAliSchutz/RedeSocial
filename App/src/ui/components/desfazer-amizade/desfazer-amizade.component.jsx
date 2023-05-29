export function DesfazerAmizades({ amigoId, onDesfazerAmizade }) {
  function handleDesfazerAmizade(e) {
    e.preventDefault();
    onDesfazerAmizade(amigoId);
  }

  return (
    <>
      <button className="button-desfazer-amizade" onClick={handleDesfazerAmizade}>Defazer Amizade</button>
    </>
  );
}
