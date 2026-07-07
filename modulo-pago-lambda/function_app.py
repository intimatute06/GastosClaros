import azure.functions as func
import datetime
import json
import logging
import random
import uuid

app = func.FunctionApp()

@app.route(route="SaldarDeuda", methods=["POST"], auth_level=func.AuthLevel.ANONYMOUS)
def SaldarDeuda(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Solicitud de pago simulado recibida.')

    try:
        req_body = req.get_json()
    except ValueError:
        return func.HttpResponse(
            json.dumps({"estado": "Fallida", "mensaje": "El cuerpo de la solicitud no es un JSON valido."}),
            status_code=400,
            mimetype="application/json"
        )

    deudor_id = req_body.get('deudorId')
    acreedor_id = req_body.get('acreedorId')
    monto = req_body.get('monto')

    if not deudor_id or not acreedor_id or not monto:
        return func.HttpResponse(
            json.dumps({
                "estado": "Fallida",
                "mensaje": "Se requieren los campos deudorId, acreedorId y monto."
            }),
            status_code=400,
            mimetype="application/json"
        )

    if monto <= 0:
        return func.HttpResponse(
            json.dumps({"estado": "Fallida", "mensaje": "El monto debe ser mayor a cero."}),
            status_code=400,
            mimetype="application/json"
        )

    # Simulacion de pasarela de pago: 90% de exito, 10% de fallo simulado
    pago_exitoso = random.random() < 0.90
    referencia = str(uuid.uuid4())
    timestamp = datetime.datetime.utcnow().isoformat() + "Z"

    if pago_exitoso:
        respuesta = {
            "estado": "Saldada",
            "referenciaTransaccion": referencia,
            "deudorId": deudor_id,
            "acreedorId": acreedor_id,
            "monto": monto,
            "fecha": timestamp,
            "mensaje": "Transferencia simulada procesada correctamente."
        }
        logging.info(f"Pago simulado exitoso. Referencia: {referencia}")
        return func.HttpResponse(json.dumps(respuesta), status_code=200, mimetype="application/json")
    else:
        respuesta = {
            "estado": "Fallida",
            "referenciaTransaccion": referencia,
            "deudorId": deudor_id,
            "acreedorId": acreedor_id,
            "monto": monto,
            "fecha": timestamp,
            "mensaje": "La transferencia simulada no pudo completarse. Intente nuevamente."
        }
        logging.warning(f"Pago simulado fallido. Referencia: {referencia}")
        return func.HttpResponse(json.dumps(respuesta), status_code=200, mimetype="application/json")
