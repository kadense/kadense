{{- define "jupyternetes.pvcs.operator.fullname" -}}
{{- printf "%s-jpvc-op" $.Release.Name }}
{{- end }}

{{- define "jupyternetes.pvcs.operator.serviceAccountName" -}}
{{- if ne .Values.jupyternetes.pvcs.operator.serviceAccount.name "" -}}
{{- printf "%s" .Values.jupyternetes.pvcs.operator.serviceAccount.name }}
{{- else -}}
{{- include "jupyternetes.pvcs.operator.fullname" . }}
{{- end }}
{{- end }}

{{- define "jupyternetes.pvcs.operator.image.repository" -}}
{{- if ne .Values.jupyternetes.pvcs.operator.image.repository "" -}}
{{- printf "%s" .Values.jupyternetes.pvcs.operator.image.repository }}
{{- else -}}
{{- printf "%s" .Values.image.repository }}
{{- end }}
{{- end }}

{{- define "jupyternetes.pvcs.operator.image.name" -}}
{{- printf "%s/%s:%s" (include "jupyternetes.pvcs.operator.image.repository" .) .Values.jupyternetes.pvcs.operator.image.name .Values.jupyternetes.pvcs.operator.image.tag }}
{{- end }}